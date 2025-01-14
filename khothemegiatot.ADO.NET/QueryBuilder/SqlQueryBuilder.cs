using khothemegiatot.ADO.NET.Attributes;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace khothemegiatot.ADO.NET.QueryBuilder;

public class SqlQueryBuilder : SqlQueryBuilderBase
{
    public static SqlQueryBuilder Select<T>(Func<T, object> selector = null) where T : ISqlTable, new()
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        T t = ReflectionCache.GetObject<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;
        builder.query.Clear();
        builder.columns.Clear();
        builder.conditions.Clear();

        if (selector is null)
        {
            builder.query.Append($"SELECT * FROM {builder.tableName}");
        }
        else
        {
            string[] columnList = selector(t).GetType().GetProperties().Select(p => p.Name).ToArray();
            if (columnList.Length == 0)
                throw new ArgumentException("No columns selected");

            builder.query.Append($"SELECT {string.Join(", ", columnList)} FROM {builder._tableName}");
        }

        return builder;
    }

    public static SqlQueryBuilder Insert<T>(T record)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        PropertyInfo[] recordProperties = ReflectionCache.GetProperties<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;
        builder.query.Clear();
        builder.parameters.Clear();
        builder.conditions.Clear();

        StringBuilder insertColumnBuilder = new StringBuilder("(");
        StringBuilder setValueBuilder = new StringBuilder("");

        foreach (PropertyInfo property in recordProperties)
        {
            SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = null;

            if (columnAttribute is null)
            {
                columnName = property.Name;
                insertColumnBuilder.Append($"{columnName}, ");
                setValueBuilder.Append($"{columnName} = @{columnName}, ");
                builder.parameters[$"@{columnName}"] = property.GetValue(record);
            }
            else
            {
                columnName = columnAttribute.ColumnName;
                if (columnAttribute.PrimaryKey && columnAttribute.AutoIncrement)
                {
                    continue;
                }
                else
                {
                    insertColumnBuilder.Append($"{columnName}, ");
                    setValueBuilder.Append($"{columnName} = @{columnName}, ");
                    builder.parameters[$"@{columnName}"] = property.GetValue(record);
                }
            }
        }

        insertColumnBuilder.Length -= 2;
        setValueBuilder.Length -= 2;
        builder.query.Append($"INSERT INTO {builder.tableName} ({insertColumnBuilder}) VALUES ({setValueBuilder})");

        return builder;
    }

    public static SqlQueryBuilder Update<T>(T record, Func<T, object> selector)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        PropertyInfo[] recordProperties = ReflectionCache.GetProperties<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;
        builder.query.Clear();
        builder.parameters.Clear();
        builder.conditions.Clear();

        StringBuilder setValueBuilder = new StringBuilder();

        foreach (PropertyInfo property in recordProperties)
        {
            SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = null;

            if (columnAttribute is null)
            {
                columnName = property.Name;
                setValueBuilder.Append($"{columnName} = @{columnName}, ");
                builder.parameters[$"@{columnName}"] = property.GetValue(record);
            }
            else
            {
                columnName = columnAttribute.ColumnName;
                if (columnAttribute.PrimaryKey && columnAttribute.AutoIncrement)
                {
                    continue;
                }
                else
                {
                    setValueBuilder.Append($"{columnName} = @{columnName}, ");
                    builder.parameters[$"@{columnName}"] = property.GetValue(record);
                }
            }
        }

        builder.query.Append($"UPDATE {builder.tableName} SET ");

        return builder;
    }

    public SqlQueryBuilder Where<T>(Expression<Func<T, bool>> predicate) where T : ISqlTable, new()
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        string where = ParseExpression(predicate.Body);

        conditions.Add(where);

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
}
