using System.Reflection;

namespace khothemegiatot.WebApi.ExtensionMethods;

public static class PropertyInfoExtensions
{
    public static T? GetValue<T>(this PropertyInfo property, object? obj)
    {
        return (T?)property.GetValue(obj);
    }
}
