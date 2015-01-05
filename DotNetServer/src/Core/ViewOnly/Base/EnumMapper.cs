using System;
using System.Collections.Generic;

namespace Core.ViewOnly.Base
{
    internal static class EnumMapper
    {
        private static readonly Cache<Type, Dictionary<string, object>> Types =
            new Cache<Type, Dictionary<string, object>>();

        public static object EnumFromString(Type enumType, string value)
        {
            var map = Types.Get(enumType, () =>
            {
                var values = Enum.GetValues(enumType);

                var newmap = new Dictionary<string, object>(values.Length, StringComparer.InvariantCultureIgnoreCase);

                foreach (var v in values)
                {
                    newmap.Add(v.ToString(), v);
                }

                return newmap;
            });


            return map[value];
        }
    }
}