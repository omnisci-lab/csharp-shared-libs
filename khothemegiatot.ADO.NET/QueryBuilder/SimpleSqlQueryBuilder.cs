using Microsoft.Data.SqlClient;

namespace OmniSciLab.Sql.QueryBuilder;


public partial class SimpleSqlQueryBuilder : SqlQueryBuilderBase
{
    protected SimpleSqlQueryBuilder() : base() { }

    public SimpleSqlQueryBuilder Where(string condition, params string[] parameters)
    {
        conditions.Add(condition);
        return this;
    }

    public SimpleSqlQueryBuilder Where(WhereClause[] whereClauses)
    {
        foreach (WhereClause clause in whereClauses)
        {
            conditions.Add($"{clause.Column} {clause.Operator} @{clause.Column}");
            _parameters.Add(new SqlParameter($"@{clause.Column}", clause.Value));
        }

        return this;
    }
}

public partial class SimpleSqlQueryBuilder
{
    public static SimpleSqlQueryBuilder Select(string tableName, params string[] columns)
    {
        SimpleSqlQueryBuilder builder = new SimpleSqlQueryBuilder();
        builder._tableName = tableName;

        string columnList = columns.Length > 0 ? string.Join(", ", columns) : "*";
        builder.query.Append($"SELECT {columnList} FROM {builder._tableName}");

        return builder;
    }

    public static SimpleSqlQueryBuilder Insert(string tableName, Dictionary<string, object> values)
    {
        SimpleSqlQueryBuilder builder = new SimpleSqlQueryBuilder();
        builder._tableName = tableName;

        builder.query.Append($"INSERT INTO {builder._tableName} ({string.Join(", ", values.Keys)}) ");
        builder.query.Append($"VALUES ({string.Join(", ", values.Keys.Select(k => $"@{k}"))})");

        foreach (var value in values)
            builder._parameters.Add(new SqlParameter($"@{value.Key}", value.Value));

        return builder;
    }

    /**
     * Update a record in the database
     * @param tableName The name of the table to update
     * @param values A dictionary of column names and values to update
     * @return The current instance of the SqlQueryBuilder
     */
    public static SimpleSqlQueryBuilder Update(string tableName, Dictionary<string, object> values)
    {
        SimpleSqlQueryBuilder builder = new SimpleSqlQueryBuilder();
        builder._tableName = tableName;

        builder.query.Append($"UPDATE {builder._tableName} SET ");

        if (values is null)
            throw new NullReferenceException();

        foreach (var value in values)
        {
            builder.query.Append($"{value.Key} = @{value.Key}, ");
            builder._parameters.Add(new SqlParameter($"@{value.Key}", value.Value));
        }

        builder.query.Length -= 2;

        return builder;
    }

    public static SimpleSqlQueryBuilder Delete(string tableName)
    {
        SimpleSqlQueryBuilder builder = new SimpleSqlQueryBuilder();
        builder._tableName = tableName;

        builder.query.Append($"DELETE FROM {builder._tableName}");

        return builder;
    }
}