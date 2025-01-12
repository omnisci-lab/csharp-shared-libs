using System.Reflection;

namespace khothemegiatot.WebApi.CQRS.ExtendedProcessing;

public class PluginInfo
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Version { get; set; }

    public Assembly? Assembly { get; set; }


    public void Execute(object? request, object? response)
    {
        if (Assembly is null)
            return;

        Type? type = Assembly.GetTypes()
            .SingleOrDefault(x => x.GetInterfaces().Any(i => i.Name == nameof(IPluginExection)));

        if (type is null)
            return;

        object? obj = Activator.CreateInstance(type);
        IPluginExection? pluginExection = obj as IPluginExection;
        if (pluginExection != null)
            pluginExection.Run(request, response);
    }
}
