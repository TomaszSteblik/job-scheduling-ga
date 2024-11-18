using System.Text.Json.Serialization;

namespace SchedulingAlgorithmModels.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Crossover
{
    CrossPointMachine,
    CrossPointDay,
    CrossPointMixed,
    CrossPointImprovedMachine,
    CrossPointImprovedMixed
}