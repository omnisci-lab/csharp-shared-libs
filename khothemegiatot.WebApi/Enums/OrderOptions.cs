using System.Text.Json.Serialization;

namespace khothemegiatot.WebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderOptions
{
    ASC, DESC
}