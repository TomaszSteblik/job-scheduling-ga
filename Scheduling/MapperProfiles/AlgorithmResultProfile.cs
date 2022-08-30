using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GeneticAlgorithm.Models;
using Scheduling.Models;
using Scheduling.Helpers;

namespace Scheduling.MapperProfiles;

public class AlgorithmResultProfile : Profile
{
    public AlgorithmResultProfile()
    {
        CreateMap<Result, AlgorithmResult>()
            .ForMember(x=>x.Schedule, 
                opt => opt.MapFrom(
                    result => GenerateScheduleFromChromosome(result.Chromosome, result.Machines)));
    }

    private static IEnumerable<object> GenerateScheduleFromChromosome(Chromosome chromosome, Machine[] machines)
    {
        var propNames = machines.Select((x, i) => x.Name ?? $"Unknown_{i}").ToArray();
            
        var asmBuilder = ReflectionHelper.GenerateTemporaryAssembly("ScheduleDynamic");
        var type = ReflectionHelper.GenerateTemporaryType(propNames, asmBuilder, "ScheduleDay");
        
        var scheduledDaysList = ReflectionHelper.GenerateTypeList(
            chromosome.Value.Select(x => x.Select(z => z.Id.ToString()).ToArray()), 
            propNames, asmBuilder, type);
        
        return scheduledDaysList;
    }
}