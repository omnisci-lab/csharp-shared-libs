using System.Reflection;

namespace OmniSciLab.WebApi.CQRS.ExtendedProcessing;

//Extended Processing Manager
public class ExtProcManager
{
    private readonly ExtProcCollection _pluginCollection;

    public ExtProcManager(ExtProcCollection pluginCollection)
    {
        _pluginCollection = pluginCollection;
    }

    public void Load()
    {
        _pluginCollection.Clear();

        string pluginFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "plugins");
        if (!Directory.Exists(pluginFolderPath))
            Directory.CreateDirectory(pluginFolderPath);

        string[] subPluginFolderPaths = Directory.GetDirectories(pluginFolderPath);

        foreach (string subPluginFolderPath in subPluginFolderPaths)
        {
            string[] filePaths = Directory.GetFiles(subPluginFolderPath)
                .Where(x => Path.GetExtension(x) == ".dll").ToArray();

            foreach (string filePath in filePaths)
            {
                Assembly assembly = Assembly.LoadFrom(filePath);
                if (assembly.GetTypes().Any(x => x.Name == "PluginExecution"))
                    _pluginCollection.Add(new PluginInfo { Assembly = assembly });
            }
        }
    }

    public List<PluginInfo> GetList()
    {
        if (_pluginCollection.Count == 0)
            Load();

        return _pluginCollection;
    }

    public void ExecutePlugins(object? request, object? response)
    {
        if (_pluginCollection.Count == 0)
            Load();

        foreach (PluginInfo plugin in _pluginCollection)
        {
            plugin.Execute(request, response);
        }
    }
}
