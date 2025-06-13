using MongoDB.Driver;
using OmniSciLab.NoSQL.MongoDB;

namespace OmniSciLab.WebApi.Repositories.MongoDB;

public class MasterRepository : IMasterRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    private bool disposedValue;

    public IMongoDatabase Database => _database;

    public MasterRepository(MongoDBConfiguration configuration)
    {
        _mongoClient = new MongoClient(configuration.ConnectionString);
        _database = _mongoClient.GetDatabase(configuration.DatabaseName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
