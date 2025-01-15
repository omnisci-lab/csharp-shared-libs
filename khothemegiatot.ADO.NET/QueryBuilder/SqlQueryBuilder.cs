using khothemegiatot.ADO.NET.Attributes;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

namespace khothemegiatot.ADO.NET.QueryBuilder;

/*** SqlQueryBuilder class
 * 
 * This class is used to build SQL queries
 * Author: Phan Xuân Chánh { Chinese Charater: 潘春正, EnglishName1: Chanh Xuan Phan, EnglishName2: StevePhan }
 *  - www.phanxuanchanh.com
 */
public class SqlQueryBuilder : SqlQueryBuilderBase
{
    private static readonly ConcurrentDictionary<(Type, string), string> _selectStatementCache
        = new ConcurrentDictionary<(Type, string), string>();

    private static readonly ConcurrentDictionary<(Type, string), string> _updateStatementCache
        = new ConcurrentDictionary<(Type, string), string>();

    private static readonly ConcurrentDictionary<Type, string> _insertStatementCache
        = new ConcurrentDictionary<Type, string>();

    protected SqlQueryBuilder() : base() { }

    /*** Select statement builder
     * 
     * @param selector
     * @return SqlQueryBuilder
     */
    public static SqlQueryBuilder Select<T>(Expression<Func<T, object>> selector = null) where T : ISqlTable, new()
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        T instance = ReflectionCache.GetObject<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        object selectorObject = null;
        string[] selectorColumnList = null;
        (Type, string) cacheKey = (ttype, null);
        if (selector is not null)
        {
            selectorObject = selector.Compile().Invoke(instance);
            cacheKey.Item2 = selector.ToString();
            selectorColumnList = selectorObject.GetType().GetProperties().Select(p => p.Name).ToArray();
        }

        string cacheValue = null;
        if (_selectStatementCache.TryGetValue(cacheKey, out cacheValue))
        {
            builder.query.Append(cacheValue);

            return builder;
        }

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;

        if (selectorObject is null)
        {
            builder.query.Append($"SELECT * FROM [{builder.tableName}]");
            _selectStatementCache.AddOrUpdate(cacheKey, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

            return builder;
        }

        if (selectorColumnList.Length == 0)
            throw new ArgumentException("No columns selected");

        PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();
        List<string> columnList = new List<string>();
        foreach (string selectorColumn in selectorColumnList)
        {
            PropertyInfo tProperty = tProperties.FirstOrDefault(p => p.Name == selectorColumn);
            if (tProperty is null)
                continue;

            SqlColumnAttribute sqlColumnAttribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
            if (sqlColumnAttribute is null)
                columnList.Add($"[{selectorColumn}]");
            else
                columnList.Add($"[{sqlColumnAttribute.ColumnName}]");
        }

        builder.query.Append($"SELECT {string.Join(", ", columnList)} FROM [{builder.tableName}]");
        _selectStatementCache.AddOrUpdate(cacheKey, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

        return builder;
    }

    /*** Insert statement builder
     * 
     * @param record
     * @return SqlQueryBuilder
     */
    public static SqlQueryBuilder Insert<T>(T record)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        PropertyInfo[] recordProperties = ReflectionCache.GetProperties<T>();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        if (_insertStatementCache.TryGetValue(ttype, out string cacheValue))
        {
            builder.query.Append(cacheValue);
            return builder;
        }

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;

        StringBuilder insertColumnBuilder = new StringBuilder("(");
        StringBuilder setValueBuilder = new StringBuilder("");

        foreach (PropertyInfo property in recordProperties)
        {
            SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = null;

            if (columnAttribute is null)
            {
                columnName = property.Name;
                insertColumnBuilder.Append($"[{columnName}], ");
                setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
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
                    insertColumnBuilder.Append($"[{columnName}], ");
                    setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
                    builder.parameters[$"@{columnName}"] = property.GetValue(record);
                }
            }
        }

        insertColumnBuilder.Length -= 2;
        setValueBuilder.Length -= 2;
        builder.query.Append($"INSERT INTO [{builder.tableName}] ({insertColumnBuilder}) VALUES ({setValueBuilder})");
        _insertStatementCache.AddOrUpdate(ttype, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

        return builder;
    }

    /*** Update statement builder
     * 
     * @param record
     * @param selector
     * @return SqlQueryBuilder
     */
    public static SqlQueryBuilder Update<T>(T record, Expression<Func<T, object>> selector = null)
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        object selectorObject = null;
        string[] selectorColumnList = null;
        (Type, string) cacheKey = (ttype, null);
        if (selector is not null)
        {
            selectorObject = selector.Compile().Invoke(record);
            cacheKey.Item2 = selector.ToString();
            selectorColumnList = selectorObject.GetType().GetProperties().Select(p => p.Name).ToArray();
        }

        string cacheValue = null;
        if (_updateStatementCache.TryGetValue(cacheKey, out cacheValue))
        {
            builder.query.Append(cacheValue);
            return builder;
        }

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;

        PropertyInfo[] recordProperties = null;
        if (selectorObject is null)
            recordProperties = ReflectionCache.GetProperties<T>();
        else
        {
            if (selectorColumnList.Length == 0)
                throw new ArgumentException("No columns selected");

            recordProperties = ReflectionCache.GetProperties<T>()
                .Where(x => selectorColumnList.Any(columnName => columnName == x.Name)).ToArray();
        }

        StringBuilder setValueBuilder = new StringBuilder();

        foreach (PropertyInfo property in recordProperties)
        {
            SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
            string columnName = null;

            if (columnAttribute is null)
            {
                columnName = property.Name;
                setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
                builder.parameters[$"@{columnName}"] = property.GetValue(record);
            }
            else
            {
                columnName = columnAttribute.ColumnName;
                if (columnAttribute.PrimaryKey)
                {
                    continue;
                }
                else
                {
                    setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
                    builder.parameters[$"@{columnName}"] = property.GetValue(record);
                }
            }
        }
        setValueBuilder.Length -= 2;
        builder.query.Append($"UPDATE [{builder.tableName}] SET {setValueBuilder}");
        _updateStatementCache.AddOrUpdate(cacheKey, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

        return builder;
    }

    /*** Delete statement builder
     * 
     * @return SqlQueryBuilder
     */
    public static SqlQueryBuilder Delete<T>()
    {
        SqlQueryBuilder builder = new SqlQueryBuilder();
        Type ttype = typeof(T);
        SqlTableAttribute tableAttribute = ttype.GetCustomAttribute<SqlTableAttribute>();

        builder.tableName = tableAttribute is null ? ttype.Name : tableAttribute.TableName;
        builder.query.Append($"DELETE FROM [{builder.tableName}]");

        return builder;
    }

    public SqlQueryBuilder Where<T>(Expression<Func<T, bool>> expression) where T : ISqlTable, new()
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();
        int anonymousParamsCount = 0;
        string where = ParseExpression(expression.Body, tProperties, ref anonymousParamsCount);

        conditions.Add(where);

        return this;
    }

    private string ParseExpression(Expression expression, PropertyInfo[] properties, ref int anonymousParamsCount)
    {
        string paramKey = null;
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                return ParseBinaryExpression(binaryExpression, properties, ref anonymousParamsCount);
            case MemberExpression memberExpression:
                PropertyInfo property = properties.FirstOrDefault(p => p.Name == memberExpression.Member.Name);
                if(property is null)
                {
                    if (!(memberExpression.Member is FieldInfo fieldInfo))
                        throw new Exception($"");

                    ConstantExpression closureExpression = (ConstantExpression) memberExpression.Expression;

                    object val = fieldInfo.GetValue(closureExpression.Value);
                    paramKey = $"@{memberExpression.Member.Name}";
                    parameters.Add(paramKey, val);

                    return paramKey;
                }

                SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();

                return columnAttribute is null ? $"[{memberExpression.Member.Name}]" : $"[{columnAttribute.ColumnName}]";
            case ConstantExpression constantExpression:
                paramKey = $"@val{++anonymousParamsCount}";
                parameters.Add(paramKey, constantExpression.Value);
                return paramKey;
            default:
                throw new NotSupportedException($"Expression type {expression.GetType()} is not supported");
        }
    }

    private string ParseBinaryExpression(BinaryExpression binaryExpression, PropertyInfo[] properties, ref int anonymousParamsCount)
    {
        var left = ParseExpression(binaryExpression.Left, properties, ref anonymousParamsCount);
        var right = ParseExpression(binaryExpression.Right, properties, ref anonymousParamsCount);
        var operatorString = GetSqlOperator(binaryExpression.NodeType);

        return $"{left} {operatorString} {right}";
    }

    private string GetSqlOperator(ExpressionType nodeType)
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
