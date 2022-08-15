using System.Text.Json.Serialization;

namespace Scheduling.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Elimination
{
    Elitism
}