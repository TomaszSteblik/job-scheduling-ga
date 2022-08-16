using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace ConsoleRunner;

internal class PersonelHelper
{
    [JsonInclude]
    [Name("name")]
    public string? Name { get; set; }
        
    [JsonInclude]
    [Name("qualifications")]
    public string? Qualifications { get; set; }
    
    [JsonInclude]
    [Name("preference_days_count")]
    public int DaysPreferenceCount { get; set; }

    [JsonInclude]
    [Name("preference_machines")]
    public string? PreferredMachineIds { get; set; }
    
    [JsonInclude]
    [Name("preference_days")]
    public string? PreferredDays { get; set; }
    
}