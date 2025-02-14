using Microsoft.Data.SqlClient;
using System.Text;

namespace OmniSciLab.Sql.QueryBuilder;

public class SqlQueryBuilderBase
{
    protected string _tableName;
    protected readonly StringBuilder query;
    protected readonly Dictionary<string, object> parameters;
    protected readonly List<SqlParameter> _parameters;

    protected readonly List<string> columns;
    protected readonly List<string> conditions;

    protected SqlQueryBuilderBase()
    {
        _tableName = null!;
        query = new StringBuilder();
        columns = new List<string>();
        conditions = new List<string>();
        parameters = new Dictionary<string, object>();
        _parameters = new List<SqlParameter>();
    }

    public string BuildQuery()
    {
        string whereClause = "";
        if (conditions.Count > 0)
        {
            whereClause = "WHERE " + string.Join(" AND ", conditions);
        }

        return $"{query.ToString()} {whereClause}";
    }

    public Dictionary<string, object> GetParameters()
    {
        return parameters;
    }
}
