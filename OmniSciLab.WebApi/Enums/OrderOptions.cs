using System.Text.Json.Serialization;

namespace OmniSciLab.WebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderOptions
{
    ASC, DESC
}