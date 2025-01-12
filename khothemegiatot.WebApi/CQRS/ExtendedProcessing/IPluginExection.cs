namespace khothemegiatot.WebApi.CQRS.ExtendedProcessing;

public interface IPluginExection
{
    void Run(object? request, object? response);
}