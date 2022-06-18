using System.Text.Json.Serialization;

namespace GeneticAlgorithm.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Elimination
{
    Elitism
}