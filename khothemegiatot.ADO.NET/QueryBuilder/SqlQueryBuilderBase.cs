using System.Text;

namespace khothemegiatot.ADO.NET.QueryBuilder;

public class SqlQueryBuilderBase
{
    protected string tableName;
    protected readonly StringBuilder query;
    protected readonly List<string> columns;
    protected readonly List<string> conditions;
    protected readonly Dictionary<string, object> parameters;

    protected SqlQueryBuilderBase()
    {
        tableName = null!;
        query = new StringBuilder();
        columns = new List<string>();
        conditions = new List<string>();
        parameters = new Dictionary<string, object>();
    }

    public string BuildQuery()
    {
        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
        }
        return query.ToString();
    }

    public Dictionary<string, object> GetParameters()
    {
        return parameters;
    }
}
