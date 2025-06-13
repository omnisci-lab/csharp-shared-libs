using System.Collections.Concurrent;

namespace OmniSciLab.WebApi.Queue;

public class QueueService<TQueueTask> where TQueueTask : IQueueTask
{
    private readonly ConcurrentQueue<TQueueTask> _taskQueue = new ConcurrentQueue<TQueueTask>();
    private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

    public void EnqueueTask(TQueueTask task)
    {
        _taskQueue.Enqueue(task);
        _signal.Release();
    }

    public async Task<TQueueTask> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _taskQueue.TryDequeue(out var task);

        return task!;
    }
}
