using System.Data;

namespace Core.ReadWrite
{
    /// <summary>
    ///     Base interface to add mapping functionality between datareader and domain entity.
    ///     This interface is used by data access layer to facilitate loading data from datatable and creating domain entity
    ///     from it
    /// </summary>
    public interface IShouldReadDataReader
    {
        void From(IDataReader dataReader);
    }
}