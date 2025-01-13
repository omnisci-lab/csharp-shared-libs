namespace khothemegiatot.ADO.NET;

public class DaoBase<T> : IDisposable
{
    protected readonly SqlExecHelper sqlExecHelper;
    private bool disposedValue;

    public DaoBase(SqlExecHelper sqlExecHelper)
    {
        this.sqlExecHelper = sqlExecHelper;
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
