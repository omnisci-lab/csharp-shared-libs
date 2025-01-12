using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

namespace khothemegiatot.ADO.NET;

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

    public async IAsyncEnumerable<T> ExecuteReaderAsync<T>(string sql, Dictionary<string, object> parameters, Func<DbDataReader, T> mapper)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters != null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return mapper(reader);
        }
    }

    public async Task<object> ExecuteScalarQueryAsync<Tscalar>(string sql, Dictionary<string, object> parameters)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters != null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        return await cmd.ExecuteScalarAsync();
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters)
    {
        await ConnectAsync();

        using SqlCommand cmd = new SqlCommand(sql, _connection);
        cmd.CommandType = CommandType.Text;

        if (parameters != null)
            parameters = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
            cmd.Parameters.AddWithValue(key, value);

        return await cmd.ExecuteNonQueryAsync();
    }
}
