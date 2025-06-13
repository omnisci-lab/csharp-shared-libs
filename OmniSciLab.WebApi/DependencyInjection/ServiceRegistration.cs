using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Reflection;
using Redis.OM;
using OmniSciLab.WebApi.Repositories.MongoDB;
using OmniSciLab.WebApi.CQRS.ExtendedProcessing;
using OmniSciLab.WebApi.CQRS.Behaviours;
using OmniSciLab.WebApi.Queue;
using OmniSciLab.WebApi.BackgroundServices;

namespace OmniSciLab.WebApi.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        string redisConnection = configuration.GetConnectionString("RedisConnection")!;
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);

        services.AddSingleton(new RedisConnectionProvider(connectionMultiplexer));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });

        return services;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExtendedProcessingBehaviour<,>));

        services.AddSingleton<ExtProcCollection>();
        services.AddScoped<ExtProcManager>();

        return services;
    }

    public static IServiceCollection AddQueue<TQueueTask>(this IServiceCollection services)
        where TQueueTask : IQueueTask
    {
        services.AddSingleton<QueueService<TQueueTask>>();
        services.AddHostedService<QueueBackgroundService<TQueueTask>>();

        return services;
    }

    public static IServiceCollection AddDataCleaner<TMasterRepository>(this IServiceCollection services)
        where TMasterRepository : IMasterRepository
    {
        PropertyInfo[] properties = typeof(TMasterRepository).GetProperties()
            .Where(x => x.PropertyType.GetInterfaces().Any(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == typeof(IAppRepository<>)))
            .ToArray();
        services.AddHostedService<DeleteDataBackgroundService<TMasterRepository>>();

        return services;
    }
}