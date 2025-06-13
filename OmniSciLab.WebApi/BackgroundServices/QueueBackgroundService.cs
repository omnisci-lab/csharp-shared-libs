using Microsoft.Extensions.Hosting;
using OmniSciLab.WebApi.Queue;

namespace OmniSciLab.WebApi.BackgroundServices;

public class QueueBackgroundService<TQueueTask> : BackgroundService
    where TQueueTask : IQueueTask
{
    private readonly QueueService<TQueueTask> _queueService;

    public QueueBackgroundService(QueueService<TQueueTask> queueService)
    {
        _queueService = queueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            IQueueTask task = await _queueService.DequeueAsync(stoppingToken);
            if (task is not null)
            {
                await task.DoTaskAsync();
            }
        }
    }
}
