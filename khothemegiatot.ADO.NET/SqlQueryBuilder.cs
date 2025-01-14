using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace khothemegiatot.ADO.NET;


public class SqlQueryBuilder
{
    private string _tableName;
    private readonly StringBuilder _query;
    private readonly List<string> _columns;
    private readonly List<string> _conditions;
    private readonly Dictionary<string, object> _parameters;

    public SqlQueryBuilder()
    {
        _query = new StringBuilder();
        _columns = new List<string>();
        _conditions = new List<string>();
        _parameters = new Dictionary<string, object>();
    }


    public static SqlQueryBuilder Select(string tableName, params string[] columns)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        builder._tableName = tableName;
        builder._query.Clear();
        builder._columns.Clear();
        builder._conditions.Clear();

        string columnList = columns.Length > 0 ? string.Join(", ", columns) : "*";
        builder._query.Append($"SELECT {columnList} FROM {builder._tableName}");

        return builder;
    }


    public static SqlQueryBuilder Select<T>(Func<T, object> selector = null) where T : ISqlTable, new()
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        T t = ReflectionCache.GetObject<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        builder._tableName = (tableAttribute is null) ? ttype.Name : tableAttribute.TableName;
        builder._query.Clear();
        builder._columns.Clear();
        builder._conditions.Clear();

        if (selector is null)
        {
            builder._query.Append($"SELECT * FROM {builder._tableName}");
        }
        else
        {
            string[] columnList = selector(t).GetType().GetProperties().Select(p => p.Name).ToArray();
            if(columnList.Length == 0)
                throw new ArgumentException("No columns selected");

            builder._query.Append($"SELECT {string.Join(", ", columnList)} FROM {builder._tableName}");
        }

        return builder;
    }

    public static SqlQueryBuilder Insert(string tableName, Dictionary<string, object> values)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        builder._tableName = tableName;
        builder._query.Clear();
        builder._parameters.Clear();
        builder._query.Append($"INSERT INTO {builder._tableName} ({string.Join(", ", values.Keys)}) ");
        builder._query.Append($"VALUES ({string.Join(", ", values.Keys.Select(k => $"@{k}"))})");

        foreach (var value in values)
        {
            builder._parameters[$"@{value.Key}"] = value.Value;
        }

        return builder;
    }

    /**
     * Update a record in the database
     * @param tableName The name of the table to update
     * @param values A dictionary of column names and values to update
     * @return The current instance of the SqlQueryBuilder
     */
    public static SqlQueryBuilder Update(string tableName, Dictionary<string, object> values)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        builder._tableName = tableName;
        builder._query.Clear();
        builder._parameters.Clear();
        builder._query.Append($"UPDATE {builder._tableName} SET ");

        if (values is null)
            throw new NullReferenceException();

        foreach (var value in values)
        {
            builder._query.Append($"{value.Key} = @{value.Key}, ");
            builder._parameters[$"@{value.Key}"] = value.Value;
        }

        builder._query.Length -= 2;

        return builder;
    }

    public static SqlQueryBuilder Delete(string tableName)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        builder._tableName = tableName;
        builder._query.Clear();
        builder._query.Append($"DELETE FROM {builder._tableName}");

        return builder;
    }

    public SqlQueryBuilder Where(string condition)
    {
        _conditions.Add(condition);
        return this;
    }

    public SqlQueryBuilder Where<T>(Expression<Func<T, bool>> predicate) where T : ISqlTable, new()
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        string where = ParseExpression(predicate.Body);

        _conditions.Add(where);

        return this;
    }

    private static string ParseExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                return ParseBinaryExpression(binaryExpression);
            case MemberExpression memberExpression:
                return memberExpression.Member.Name;
            case ConstantExpression constantExpression:
                return constantExpression.Value.ToString();
            default:
                throw new NotSupportedException($"Expression type {expression.GetType()} is not supported");
        }
    }

    private static string ParseBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = ParseExpression(binaryExpression.Left);
        var right = ParseExpression(binaryExpression.Right);
        var operatorString = GetSqlOperator(binaryExpression.NodeType);

        return $"{left} {operatorString} {right}";
    }

    private static string GetSqlOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.LessThan => "<",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            _ => throw new NotSupportedException($"Operator {nodeType} not supported")
        };
    }


    public string BuildQuery()
    {
        if (_conditions.Count > 0)
        {
            _query.Append(" WHERE " + string.Join(" AND ", _conditions));
        }
        return _query.ToString();
    }

    public Dictionary<string, object> GetParameters()
    {
        return _parameters;
    }
}