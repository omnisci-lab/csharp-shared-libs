using System.Linq.Expressions;

namespace khothemegiatot.ADO.NET;

public class DaoBase<T> : IDisposable where T : ISqlTable , new()
{
    protected readonly SqlExecHelper sqlExecHelper;
    private bool disposedValue;

    public DaoBase(SqlExecHelper sqlExecHelper)
    {
        this.sqlExecHelper = sqlExecHelper;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null)
    {
        SqlQueryBuilder builder = null;
        if (typeof(T) == typeof(SqlTableWithTimestamp))
            builder = SqlQueryBuilder.Select<T>().Where<SqlTableWithTimestamp>(x => x.DeletedAt == null)
                .Where(expression);
        else
            builder = SqlQueryBuilder.Select<T>().Where(expression);

        IAsyncEnumerable<T> asyncEnumerable = sqlExecHelper
            .ExecuteReaderAsync(builder, reader => SqlMapper.MapRow<T>(reader));

        return await asyncEnumerable.FirstOrDefaultAsync();
    }

    public IAsyncEnumerable<T> GetListAsync(Expression<Func<T, bool>> expression = null)
    {
        SqlQueryBuilder builder = null;
        if (typeof(T) == typeof(SqlTableWithTimestamp))
            builder = SqlQueryBuilder.Select<T>().Where<SqlTableWithTimestamp>(x => x.DeletedAt == null)
                .Where(expression);
        else
            builder = SqlQueryBuilder.Select<T>().Where(expression);

        return sqlExecHelper.ExecuteReaderAsync(builder, reader => SqlMapper.MapRow<T>(reader));
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }


            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
