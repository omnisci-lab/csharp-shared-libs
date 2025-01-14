using Microsoft.Data.SqlClient;
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
        PropertyInfo[] tProperties = typeof(T).GetProperties();

        T instance = new T();
        foreach (PropertyInfo tProperty in tProperties)
        {
            SqlColumnAttribute attribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = attribute is null ? tProperty.Name : attribute.ColumnName;

            tProperty.SetValue(instance, reader[attribute.ColumnName]);

            if (reader.HasColumn(attribute.ColumnName) && !reader.IsDBNull(reader.GetOrdinal(attribute.ColumnName)))
            {
                object value = reader.GetValue(reader.GetOrdinal(attribute.ColumnName));
                tProperty.SetValue(instance, Convert.ChangeType(value, tProperty.PropertyType));
            }
        }

        return instance;
    }
}
