using Japanese.Core.RepositoryBase.MongoDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace khothemegiatot.WebApi.BackgroundServices;

public class DeleteDataBackgroundService<TMasterRepository> : BackgroundService
    where TMasterRepository : IMasterRepository
{
    private readonly IServiceProvider _serviceProvider;

    public DeleteDataBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
