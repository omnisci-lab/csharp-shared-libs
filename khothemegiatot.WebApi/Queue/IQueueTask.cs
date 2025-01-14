namespace khothemegiatot.WebApi.Queue;

public interface IQueueTask
{
    string TaskID { get; }

    Task DoTaskAsync();
}
