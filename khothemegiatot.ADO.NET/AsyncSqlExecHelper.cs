using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using OmniSciLab.Sql.QueryBuilder;

namespace OmniSciLab.Sql;

public partial class SqlExecHelper
{
    public async Task ConnectAsync()
    {
        if (_connection != null && _connection.State != ConnectionState.Open)
            await _connection.OpenAsync();
    }

    public void DisconnectAsync()
    {
        if (_connection != null && _connection.State != ConnectionState.Closed)
            _connection.Close();
    }

    public async IAsyncEnumerable<T> ExecuteReaderAsync<T>(string sql, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapper)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters == null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return mapper(reader);
        }
    }

    public async Task<Tscalar?> ExecuteScalarQueryAsync<Tscalar>(string sql, Dictionary<string, object>? parameters = null)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters == null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        object? val = await cmd.ExecuteScalarAsync();

        return val is null ? default : (Tscalar)val;
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters = null)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters == null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async IAsyncEnumerable<T> ExecuteReaderAsync<T>(SqlQueryBuilderBase builder, Func<SqlDataReader, T> mapper)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(builder.GetParameters().ToArray());

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return mapper(reader);
        }
    }

    public async Task<Tscalar?> ExecuteScalarQueryAsync<Tscalar>(SqlQueryBuilderBase builder)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(builder.GetParameters().ToArray());

        object? val = await cmd.ExecuteScalarAsync();

        return val is null ? default : (Tscalar)val;
    }

    public async Task<int> ExecuteNonQueryAsync(SqlQueryBuilderBase builder)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddRange(builder.GetParameters().ToArray());

        return await cmd.ExecuteNonQueryAsync();
    }
}
