using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.ViewOnly.Base
{
    internal class PocoData
    {
        private static readonly Cache<Type, PocoData> PocoDatas = new Cache<Type, PocoData>();
        private static readonly List<Func<object, object>> Converters = new List<Func<object, object>>();
        private static readonly MethodInfo FnGetValue = typeof (IDataRecord).GetMethod("GetValue", new[] {typeof (int)});
        private static readonly MethodInfo FnIsDbNull = typeof (IDataRecord).GetMethod("IsDBNull");

        private static readonly FieldInfo FldConverters = typeof (PocoData).GetField("_converters",
            BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic);

        private static readonly MethodInfo FnListGetItem =
            typeof (List<Func<object, object>>).GetProperty("Item").GetGetMethod();

        private static readonly MethodInfo FnInvoke = typeof (Func<object, object>).GetMethod("Invoke");

        private readonly Cache<Tuple<string, string, int, int>, Delegate> _pocoFactories =
            new Cache<Tuple<string, string, int, int>, Delegate>();

        public Type PocoType;

        public PocoData(Type t)
        {
            PocoType = t;

            // Get the mapper for this type
            var mapper = Mappers.GetMapper(t);

            // Get the table info
            ViewInfo = mapper.GetViewInfo(t);

            // Work out bound properties
            Columns = new Dictionary<string, PocoColumn>(StringComparer.OrdinalIgnoreCase);
            foreach (var pc in from pi in t.GetProperties()
                let ci = mapper.GetColumnInfo(pi)
                where ci != null
                select new PocoColumn
                {
                    PropertyInfo = pi,
                    ColumnName = ci.ColumnName,
                    NoSearch = ci.DoNotSearch,
                    ForceToUtc = ci.ForceToUtc
                })
            {
                // Store it
                Columns.Add(pc.ColumnName, pc);
            }

            // Build column list for automatic select
            ResultColumns = (from c in Columns select c.Key).ToArray();
            SearchableColumns = (from c in Columns where !c.Value.NoSearch select c.Key).ToArray();
        }

        public ViewInfo ViewInfo { get; private set; }
        public string[] ResultColumns { get; private set; }
        public string[] SearchableColumns { get; private set; }
        public Dictionary<string, PocoColumn> Columns { get; private set; }

        public static PocoData ForType(Type t)
        {
            if (t == typeof (ExpandoObject))
                throw new InvalidOperationException("Can't use dynamic types with this method");
            return PocoDatas.Get(t, () => new PocoData(t));
        }

        private static bool IsIntegralType(Type t)
        {
            var tc = Type.GetTypeCode(t);
            return tc >= TypeCode.SByte && tc <= TypeCode.UInt64;
        }

        // Create factory function that can convert a IDataReader record into a POCO
        public Delegate GetFactory(string sql, string connString, int firstColumn, int countColumns, IDataReader r)
        {
            // Check cache
            var key = Tuple.Create(sql, connString, firstColumn, countColumns);

            return _pocoFactories.Get(key, () =>
            {
                // Create the method
                var m = new DynamicMethod("petapoco_factory_" + _pocoFactories.Count, PocoType,
                    new[] {typeof (IDataReader)}, true);
                var il = m.GetILGenerator();
                var mapper = Mappers.GetMapper(PocoType);

                if (PocoType == typeof (object))
                {
                    // var poco=new T()
                    il.Emit(OpCodes.Newobj, typeof (ExpandoObject).GetConstructor(Type.EmptyTypes)); // obj

                    var fnAdd = typeof (IDictionary<string, object>).GetMethod("Add");

                    // Enumerate all fields generating a set assignment for the column
                    for (var i = firstColumn; i < firstColumn + countColumns; i++)
                    {
                        var srcType = r.GetFieldType(i);

                        il.Emit(OpCodes.Dup); // obj, obj
                        il.Emit(OpCodes.Ldstr, r.GetName(i)); // obj, obj, fieldname

                        // Get the converter
                        var converter = mapper.GetFromDbConverter(null, srcType);

                        /*
						if (ForceDateTimesToUtc && converter == null && srcType == typeof(DateTime))
							converter = delegate(object src) { return new DateTime(((DateTime)src).Ticks, DateTimeKind.Utc); };
						 */

                        // Setup stack for call to converter
                        AddConverterToStack(il, converter);

                        // r[i]
                        il.Emit(OpCodes.Ldarg_0); // obj, obj, fieldname, converter?,    rdr
                        il.Emit(OpCodes.Ldc_I4, i); // obj, obj, fieldname, converter?,  rdr,i
                        il.Emit(OpCodes.Callvirt, FnGetValue); // obj, obj, fieldname, converter?,  value

                        // Convert DBNull to null
                        il.Emit(OpCodes.Dup); // obj, obj, fieldname, converter?,  value, value
                        il.Emit(OpCodes.Isinst, typeof (DBNull));
                        // obj, obj, fieldname, converter?,  value, (value or null)
                        var lblNotNull = il.DefineLabel();
                        il.Emit(OpCodes.Brfalse_S, lblNotNull); // obj, obj, fieldname, converter?,  value
                        il.Emit(OpCodes.Pop); // obj, obj, fieldname, converter?
                        if (converter != null)
                            il.Emit(OpCodes.Pop); // obj, obj, fieldname, 
                        il.Emit(OpCodes.Ldnull); // obj, obj, fieldname, null
                        if (converter != null)
                        {
                            var lblReady = il.DefineLabel();
                            il.Emit(OpCodes.Br_S, lblReady);
                            il.MarkLabel(lblNotNull);
                            il.Emit(OpCodes.Callvirt, FnInvoke);
                            il.MarkLabel(lblReady);
                        }
                        else
                        {
                            il.MarkLabel(lblNotNull);
                        }

                        il.Emit(OpCodes.Callvirt, fnAdd);
                    }
                }
                else if (PocoType.IsValueType || PocoType == typeof (string) || PocoType == typeof (byte[]))
                {
                    // Do we need to install a converter?
                    var srcType = r.GetFieldType(0);
                    var converter = GetConverter(mapper, null, srcType, PocoType);

                    // "if (!rdr.IsDBNull(i))"
                    il.Emit(OpCodes.Ldarg_0); // rdr
                    il.Emit(OpCodes.Ldc_I4_0); // rdr,0
                    il.Emit(OpCodes.Callvirt, FnIsDbNull); // bool
                    var lblCont = il.DefineLabel();
                    il.Emit(OpCodes.Brfalse_S, lblCont);
                    il.Emit(OpCodes.Ldnull); // null
                    var lblFin = il.DefineLabel();
                    il.Emit(OpCodes.Br_S, lblFin);

                    il.MarkLabel(lblCont);

                    // Setup stack for call to converter
                    AddConverterToStack(il, converter);

                    il.Emit(OpCodes.Ldarg_0); // rdr
                    il.Emit(OpCodes.Ldc_I4_0); // rdr,0
                    il.Emit(OpCodes.Callvirt, FnGetValue); // value

                    // Call the converter
                    if (converter != null)
                        il.Emit(OpCodes.Callvirt, FnInvoke);

                    il.MarkLabel(lblFin);
                    il.Emit(OpCodes.Unbox_Any, PocoType); // value converted
                }
                else
                {
                    // var poco=new T()
                    il.Emit(OpCodes.Newobj,
                        PocoType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                            null, new Type[0], null));

                    // Enumerate all fields generating a set assignment for the column
                    for (var i = firstColumn; i < firstColumn + countColumns; i++)
                    {
                        // Get the PocoColumn for this db column, ignore if not known
                        PocoColumn pc;
                        if (!Columns.TryGetValue(r.GetName(i), out pc))
                            continue;

                        // Get the source type for this column
                        var srcType = r.GetFieldType(i);
                        var dstType = pc.PropertyInfo.PropertyType;

                        // "if (!rdr.IsDBNull(i))"
                        il.Emit(OpCodes.Ldarg_0); // poco,rdr
                        il.Emit(OpCodes.Ldc_I4, i); // poco,rdr,i
                        il.Emit(OpCodes.Callvirt, FnIsDbNull); // poco,bool
                        var lblNext = il.DefineLabel();
                        il.Emit(OpCodes.Brtrue_S, lblNext); // poco

                        il.Emit(OpCodes.Dup); // poco,poco

                        // Do we need to install a converter?
                        var converter = GetConverter(mapper, pc, srcType, dstType);

                        // Fast
                        var handled = false;
                        if (converter == null)
                        {
                            var valuegetter = typeof (IDataRecord).GetMethod("Get" + srcType.Name,
                                new[] {typeof (int)});
                            if (valuegetter != null
                                && valuegetter.ReturnType == srcType
                                &&
                                (valuegetter.ReturnType == dstType ||
                                 valuegetter.ReturnType == Nullable.GetUnderlyingType(dstType)))
                            {
                                il.Emit(OpCodes.Ldarg_0); // *,rdr
                                il.Emit(OpCodes.Ldc_I4, i); // *,rdr,i
                                il.Emit(OpCodes.Callvirt, valuegetter); // *,value

                                // Convert to Nullable
                                if (Nullable.GetUnderlyingType(dstType) != null)
                                {
                                    il.Emit(OpCodes.Newobj,
                                        dstType.GetConstructor(new[] {Nullable.GetUnderlyingType(dstType)}));
                                }

                                il.Emit(OpCodes.Callvirt, pc.PropertyInfo.GetSetMethod(true)); // poco
                                handled = true;
                            }
                        }

                        // Not so fast
                        if (!handled)
                        {
                            // Setup stack for call to converter
                            AddConverterToStack(il, converter);

                            // "value = rdr.GetValue(i)"
                            il.Emit(OpCodes.Ldarg_0); // *,rdr
                            il.Emit(OpCodes.Ldc_I4, i); // *,rdr,i
                            il.Emit(OpCodes.Callvirt, FnGetValue); // *,value

                            // Call the converter
                            if (converter != null)
                                il.Emit(OpCodes.Callvirt, FnInvoke);

                            // Assign it
                            il.Emit(OpCodes.Unbox_Any, pc.PropertyInfo.PropertyType); // poco,poco,value
                            il.Emit(OpCodes.Callvirt, pc.PropertyInfo.GetSetMethod(true)); // poco
                        }

                        il.MarkLabel(lblNext);
                    }

                    var fnOnLoaded = RecurseInheritedTypes(PocoType,
                        x =>
                            x.GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                null, new Type[0], null));
                    if (fnOnLoaded != null)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Callvirt, fnOnLoaded);
                    }
                }

                il.Emit(OpCodes.Ret);

                // Cache it, return it
                return m.CreateDelegate(Expression.GetFuncType(typeof (IDataReader), PocoType));
            }
                );
        }

        private static void AddConverterToStack(ILGenerator il, Func<object, object> converter)
        {
            if (converter == null) return;

            // Add the converter
            var converterIndex = Converters.Count;
            Converters.Add(converter);

            // Generate IL to push the converter onto the stack
            il.Emit(OpCodes.Ldsfld, FldConverters);
            il.Emit(OpCodes.Ldc_I4, converterIndex);
            il.Emit(OpCodes.Callvirt, FnListGetItem); // Converter
        }

        private static Func<object, object> GetConverter(IMapper mapper, PocoColumn pc, Type srcType, Type dstType)
        {
            // Get converter from the mapper
            if (pc != null)
            {
                var converter = mapper.GetFromDbConverter(pc.PropertyInfo, srcType);
                if (converter != null) return converter;
            }

            // Standard DateTime->Utc mapper
            if (pc != null && pc.ForceToUtc && srcType == typeof (DateTime) &&
                (dstType == typeof (DateTime) || dstType == typeof (DateTime?)))
            {
                return src => new DateTime(((DateTime) src).Ticks, DateTimeKind.Utc);
            }

            // Forced type conversion including integral types -> enum
            if (dstType.IsEnum && IsIntegralType(srcType))
            {
                if (srcType != typeof (int))
                {
                    return src => Convert.ChangeType(src, typeof (int), null);
                }
            }
            else if (!dstType.IsAssignableFrom(srcType))
            {
                if (dstType.IsEnum && srcType == typeof (string))
                {
                    return src => EnumMapper.EnumFromString(dstType, (string) src);
                }
                return src => Convert.ChangeType(src, dstType, null);
            }

            return null;
        }


        private static T RecurseInheritedTypes<T>(Type t, Func<Type, T> cb)
        {
            while (t != null)
            {
                var info = cb(t);
                if (info != null) return info;
                t = t.BaseType;
            }
            return default(T);
        }


        internal static void FlushCaches()
        {
            PocoDatas.Flush();
        }
    }
}