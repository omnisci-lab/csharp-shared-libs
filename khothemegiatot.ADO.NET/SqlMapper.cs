using khothemegiatot.ADO.NET.Attributes;
using Microsoft.Data.SqlClient;
using OmniSciLab.Sql.Cache;
using System.Reflection;

namespace khothemegiatot.ADO.NET;

public class SqlMapper
{
    /***
     * Map a row from a SqlDataReader to an object of type T
     * 
     * @param reader SqlDataReader
     * @return T
     */
    public static T MapRow<T>(SqlDataReader reader) where T : ISqlTable, new()
    {
        T instance = new T();
        PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();

        foreach (PropertyInfo tProperty in tProperties)
        {
            SqlColumnAttribute? attribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = attribute is null ? tProperty.Name : attribute.ColumnName;

            if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                object value = reader.GetValue(reader.GetOrdinal(columnName));
                tProperty.SetValue(instance, Convert.ChangeType(value, tProperty.PropertyType));
            }
        }

        return instance;
    }
}
