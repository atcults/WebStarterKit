using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Core.ViewOnly.Base;
using Core.ViewOnly.Impl;

namespace Core.ViewOnly
{
    /// <summary>
    ///     This static manages registation of IMapper instances with PetaPoco
    /// </summary>
    public static class Mappers
    {
        private static readonly Dictionary<object, IMapper> RegisteredMappers = new Dictionary<object, IMapper>();
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        /// <summary>
        ///     Registers a mapper for all types in a specific assembly
        /// </summary>
        /// <param name="assembly">The assembly whose types are to be managed by this mapper</param>
        /// <param name="mapper">The IMapper implementation</param>
        public static void Register(Assembly assembly, IMapper mapper)
        {
            RegisterInternal(assembly, mapper);
        }

        /// <summary>
        ///     Registers a mapper for a single POCO type
        /// </summary>
        /// <param name="type">The type to be managed by this mapper</param>
        /// <param name="mapper">The IMapper implementation</param>
        public static void Register(Type type, IMapper mapper)
        {
            RegisterInternal(type, mapper);
        }

        /// <summary>
        ///     Remove all mappers for all types in a specific assembly
        /// </summary>
        /// <param name="assembly">The assembly whose mappers are to be revoked</param>
        public static void Revoke(Assembly assembly)
        {
            RevokeInternal(assembly);
        }

        /// <summary>
        ///     Remove the mapper for a specific type
        /// </summary>
        /// <param name="type">The type whose mapper is to be removed</param>
        public static void Revoke(Type type)
        {
            RevokeInternal(type);
        }

        /// <summary>
        ///     Revoke an instance of a mapper
        /// </summary>
        /// <param name="mapper">The IMapper to be revkoed</param>
        public static void Revoke(IMapper mapper)
        {
            Lock.EnterWriteLock();
            try
            {
                foreach (var i in RegisteredMappers.Where(kvp => kvp.Value == mapper).ToList())
                    RegisteredMappers.Remove(i.Key);
            }
            finally
            {
                Lock.ExitWriteLock();
                FlushCaches();
            }
        }

        /// <summary>
        ///     Retrieve the IMapper implementation to be used for a specified POCO type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IMapper GetMapper(Type t)
        {
            Lock.EnterReadLock();
            try
            {
                IMapper val;
                if (RegisteredMappers.TryGetValue(t, out val))
                    return val;
                if (RegisteredMappers.TryGetValue(t.Assembly, out val))
                    return val;

                return Singleton<StandardMapper>.Instance;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        private static void RegisterInternal(object typeOrAssembly, IMapper mapper)
        {
            Lock.EnterWriteLock();
            try
            {
                RegisteredMappers.Add(typeOrAssembly, mapper);
            }
            finally
            {
                Lock.ExitWriteLock();
                FlushCaches();
            }
        }

        private static void RevokeInternal(object typeOrAssembly)
        {
            Lock.EnterWriteLock();
            try
            {
                RegisteredMappers.Remove(typeOrAssembly);
            }
            finally
            {
                Lock.ExitWriteLock();
                FlushCaches();
            }
        }

        private static void FlushCaches()
        {
            // Whenever a mapper is registered or revoked, we have to assume any generated code is no longer valid.
            // Since this should be a rare occurance, the simplest approach is to simply dump everything and start over.
            MultiPocoFactory.FlushCaches();
            PocoData.FlushCaches();
        }
    }
}