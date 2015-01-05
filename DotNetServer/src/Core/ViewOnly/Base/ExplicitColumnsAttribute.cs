using System;

namespace Core.ViewOnly.Base
{
    /// <summary>
    ///     Poco classes marked with the Explicit attribute require all column properties to
    ///     be marked with the Column attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitColumnsAttribute : System.Attribute
    {
    }
}