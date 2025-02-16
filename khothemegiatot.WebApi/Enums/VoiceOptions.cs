using System.Text.Json.Serialization;

namespace khothemegiatot.WebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoiceOptions
{
    MaleVoiceSound, FemaleVoiceSound
}
