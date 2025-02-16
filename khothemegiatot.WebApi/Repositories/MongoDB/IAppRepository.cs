using khothemegiatot.NoSQL.MongoDB;
using khothemegiatot.WebApi.Models;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace khothemegiatot.WebApi.Repositories.MongoDB;

public interface IAppRepository<TModel> where TModel : MongoDBModel
{
    Task<PagedResult<TModel>> GetPaginatedAsync(Pagination pagination);
    Task<TModel> GetAsync(ObjectId objectId);
    Task InsertAsync(params TModel[] models);
    Task UpdateAsync(Expression<Func<TModel, object>> filterEq, object filterEqVal, Dictionary<string, object> updates);
    Task DeleteAsync(Expression<Func<TModel, bool>> filter, bool forceDelete);
    Task<bool> Exists(Expression<Func<TModel, bool>> filter);
}