namespace khothemegiatot.ADO.NET;

[AttributeUsage(AttributeTargets.Property)]
public class SqlColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public SqlColumnAttribute(string columnName)
    {
        if(string.IsNullOrEmpty(columnName))
            throw new ArgumentNullException(nameof(columnName));

        ColumnName = columnName;
    }
}