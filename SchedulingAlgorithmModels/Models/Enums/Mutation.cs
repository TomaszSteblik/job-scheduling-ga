using System.Text.Json.Serialization;

namespace SchedulingAlgorithmModels.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Mutation
{
    Random
}