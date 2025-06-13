using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OmniSciLab.NoSQL.MongoDB;
using OmniSciLab.WebApi.Models;
using System.Linq.Expressions;

namespace OmniSciLab.WebApi.Repositories.MongoDB;

public class AppRepository<TModel> : IAppRepository<TModel> where TModel : MongoDBModel
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<TModel> _collection;
    private readonly string _collectionName;

    protected MongoDBHelper<TModel> MongoDBHelper { get => new MongoDBHelper<TModel>(_database, _collectionName); }

    public IMongoCollection<TModel> Collection => _collection;

    public AppRepository(IMongoDatabase database, string collectionName)
    {
        _database = database;
        _collectionName = collectionName;
        _collection = _database.GetCollection<TModel>(collectionName);
    }

    //public async Task<PagedResult<TModel>> GetPaginatedAsync(Pagination pagination)
    //    => await MongoDBHelper.GetPaginatedAsync(pagination);

    public async Task<PagedResult<TModel>> GetPaginatedAsync(Pagination pagination)
    {
        PagedResult<TModel> pagedResult = new PagedResult<TModel>();
        pagedResult.PageSize = pagination.PageSize;
        pagination.Page = pagination.Page;

        pagedResult.Items = await _collection.AsQueryable()
            .Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();

        pagedResult.TotalItems = await _collection.AsQueryable()
            .CountAsync();

        return pagedResult;
    }

    public async Task<TModel> GetAsync(ObjectId id)
        => await _collection.AsQueryable().Where(x => x.Id == id).SingleOrDefaultAsync();

    public async Task InsertAsync(params TModel[] models)
    {
        foreach (TModel model in models)
            model.CreatedAt = DateTime.Now;

        await MongoDBHelper.InsertAsync(models);
    }

    public async Task UpdateAsync(Expression<Func<TModel, object>> filterEq, object filterEqVal, Dictionary<string, object> updates)
    {
        KeyValuePair<string, object> updatedAtPair = updates.Where(x => x.Key == "updatedAt").SingleOrDefault();
        if (updatedAtPair.Key is null)
            updates.Add("updatedAt", DateTime.Now);

        if (updatedAtPair.Value is null)
            updates["updatedAt"] = DateTime.Now;

        await MongoDBHelper.UpdateAsync(filterEq, filterEqVal, updates);
    }

    public async Task DeleteAsync(Expression<Func<TModel, bool>> filter, bool forceDelete)
    {
        if (forceDelete)
        {
            await _collection.DeleteOneAsync(filter);
        }
        else
        {
            TModel model = await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
            await MongoDBHelper.UpdateAsync(f => f.Id, model.Id, new Dictionary<string, object> {
                { "deleteAt", DateTime.Now}
            });
        }
    }

    public async Task<bool> Exists(Expression<Func<TModel, bool>> filter)
    => await _collection.Find(filter).AnyAsync();
}