using System.Text.Json.Serialization;

namespace khothemegiatot.WebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExecStatus
{
    Success, NotFound, AlreadyExists, Invalid, Failed, Exception
}