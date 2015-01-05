using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Extensions
{
    public static class ReflectionHelper
    {
        public static MethodInfo[] GetPublicNoArgMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => (m.GetParameters().Length == 0) && (m.MemberType == MemberTypes.Method)).ToArray();
        }

        public static PropertyInfo[] GetPublicGetProperties(this Type type)
        {
            return type.FindMembers(MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance, (m, f) => ((PropertyInfo) m).CanRead, null).Cast<PropertyInfo>().ToArray();
        }

        public static MethodInfo FindModelMethodByName(MethodInfo[] getMethods, string nameToSearch)
        {
            var getName = "Get" + nameToSearch;
            return getMethods.FirstOrDefault(m => (String.Compare(m.Name, getName, StringComparison.Ordinal) == 0) || (String.Compare(m.Name, nameToSearch, StringComparison.Ordinal) == 0));
        }

        public static PropertyInfo FindModelPropertyByName(PropertyInfo[] modelProperties, string nameToSearch)
        {
            return modelProperties.FirstOrDefault(prop => String.Compare(prop.Name, nameToSearch, StringComparison.Ordinal) == 0);
        }

        public static PropertyInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression) expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var propertyInfo = ((MemberExpression) expressionToCheck).Member as PropertyInfo;
                        return propertyInfo;
                    default:
                        done = true;
                        break;
                }
            }

            return null;
        }

        public static void Update(this object src, object dest, bool preserve = false)
        {
            var type = src.GetType();
            while (type != null)
            {
                UpdateForType(type, src, dest, preserve);
                type = type.BaseType;
            }
        }

        private static void UpdateForType(IReflect type, object source, object destination, bool preserve)
        {
            var myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var fi in myObjectFields)
            {
                var srcVal = fi.GetValue(source);
                
                if (!preserve)
                {
                    fi.SetValue(destination, srcVal);
                    continue;
                }

                var destVal = fi.GetValue(destination);
                var defaultGeneratorType = typeof(DefaultGenerator<>).MakeGenericType(fi.FieldType);
                var destDefault = defaultGeneratorType.InvokeMember("GetDefault", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[0]);
                
                if (fi.FieldType == typeof (string))
                {
                    if (destVal != null && destVal.ToString().Equals(string.Empty)) destVal = null;
                }

                if (destVal != null && !destVal.Equals(destDefault)) continue;
                fi.SetValue(destination, srcVal);
            }
        }

        public class DefaultGenerator<T>
        {
            public static T GetDefault()
            {
                return default(T);
            }
        }
    }
}