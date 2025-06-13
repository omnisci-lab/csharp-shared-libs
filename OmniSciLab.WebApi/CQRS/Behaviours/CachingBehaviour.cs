using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OmniSciLab.WebApi.CQRS.Cache;
using System.Text;
using System.Text.Json;

namespace OmniSciLab.WebApi.CQRS.Behaviours;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : ICacheableQuery
    where TResponse : class
{
    private readonly IDistributedCache _cache;

    public CachingBehavior(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();

        byte[]? cacheValue = await _cache.GetAsync(request.CacheKey!);
        if (cacheValue != null && !request.RefreshCache)
            return JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(cacheValue));

        if (cacheValue != null && request.RefreshCache)
            await _cache.RemoveAsync(request.CacheKey!);

        TResponse? response = await next();
        cacheValue = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await _cache.SetAsync(request.CacheKey!, cacheValue, options);

        return response;
    }
}