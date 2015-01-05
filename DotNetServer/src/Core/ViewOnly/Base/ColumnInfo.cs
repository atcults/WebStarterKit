using System.Reflection;

namespace Core.ViewOnly.Base
{
    /// <summary>
    ///     Hold information about a column in the database.
    /// </summary>
    /// <remarks>
    ///     Typically ColumnInfo is automatically populated from the attributes on a POCO object and it's properties. It can
    ///     however also be returned from the IMapper interface to provide your owning bindings between the DB and your POCOs.
    /// </remarks>
    public class ColumnInfo
    {
        /// <summary>
        ///     The SQL name of the column
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     True will prevent this column from being where condition
        /// </summary>
        public bool DoNotSearch { get; set; }

        /// <summary>
        ///     True if time and date values returned through this column should be forced to UTC DateTimeKind. (no conversion is
        ///     applied - the Kind of the DateTime property
        ///     is simply set to DateTimeKind.Utc instead of DateTimeKind.Unknown.
        /// </summary>
        public bool ForceToUtc { get; set; }

        /// <summary>
        ///     Creates and populates a ColumnInfo from the attributes of a POCO property.
        /// </summary>
        /// <param name="pi">The property whose column info is required</param>
        /// <returns>A ColumnInfo instance</returns>
        public static ColumnInfo FromProperty(PropertyInfo pi)
        {
            // Check if declaring poco has [Explicit] attribute
            var explicitColumns = pi.DeclaringType != null &&
                                  pi.DeclaringType.GetCustomAttributes(typeof (ExplicitColumnsAttribute), true).Length >
                                  0;

            // Check for [Column]/[Ignore] Attributes
            var colAttrs = pi.GetCustomAttributes(typeof (ColumnAttribute), true);
            if (explicitColumns)
            {
                if (colAttrs.Length == 0)
                    return null;
            }

            var ci = new ColumnInfo();

            // Read attribute
            if (colAttrs.Length > 0)
            {
                var colattr = (ColumnAttribute) colAttrs[0];

                if (colattr.Ignore) return null;

                ci.ColumnName = colattr.Name ?? pi.Name;
                ci.DoNotSearch = colattr.NoSearch;
                ci.ForceToUtc = colattr.ForceToUtc;
            }
            else
            {
                ci.ColumnName = pi.Name;
                ci.ForceToUtc = false;
                ci.DoNotSearch = false;
            }

            return ci;
        }
    }
}