using System.Text.Json.Serialization;

namespace SchedulingAlgorithmModels.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Elimination
{
    Elitism
}