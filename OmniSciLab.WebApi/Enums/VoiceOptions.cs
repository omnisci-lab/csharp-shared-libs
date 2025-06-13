using System.Text.Json.Serialization;

namespace OmniSciLab.WebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoiceOptions
{
    MaleVoiceSound, FemaleVoiceSound
}
