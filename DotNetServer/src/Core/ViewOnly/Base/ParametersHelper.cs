using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.ViewOnly.Base
{
    internal static class ParametersHelper
    {
        // Helper to handle named parameters from object properties

        private static readonly Regex RxParams = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        public static string ProcessParams(string sql, object[] argsSrc, List<object> argsDest)
        {
            return RxParams.Replace(sql, m =>
            {
                var param = m.Value.Substring(1);

                object argVal;

                int paramIndex;
                if (int.TryParse(param, out paramIndex))
                {
                    // Numbered parameter
                    if (paramIndex < 0 || paramIndex >= argsSrc.Length)
                        throw new ArgumentOutOfRangeException(
                            string.Format("Parameter '@{0}' specified but only {1} parameters supplied (in `{2}`)",
                                paramIndex, argsSrc.Length, sql));
                    argVal = argsSrc[paramIndex];
                }
                else
                {
                    // Look for a property on one of the arguments with this name
                    var found = false;
                    argVal = null;
                    foreach (var o in argsSrc)
                    {
                        var pi = o.GetType().GetProperty(param);
                        if (pi == null) continue;
                        argVal = pi.GetValue(o, null);
                        found = true;
                        break;
                    }

                    if (!found)
                        throw new ArgumentException(
                            string.Format(
                                "Parameter '@{0}' specified but none of the passed arguments have a property with this name (in '{1}')",
                                param, sql));
                }

                // Expand collections to parameter lists
                if ((argVal as IEnumerable) != null &&
                    (argVal as string) == null &&
                    (argVal as byte[]) == null)
                {
                    var sb = new StringBuilder();
                    foreach (var i in argVal as IEnumerable)
                    {
                        sb.Append((sb.Length == 0 ? "@" : ",@") + argsDest.Count.ToString(CultureInfo.InvariantCulture));
                        argsDest.Add(i);
                    }
                    return sb.ToString();
                }

                argsDest.Add(argVal);
                return "@" + (argsDest.Count - 1).ToString(CultureInfo.InvariantCulture);
            });
        }
    }
}