using AutoMapper;
using GeneticAlgorithm.Models;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class AlgorithmSettingsProfile : Profile
{
    public AlgorithmSettingsProfile()
    {
        CreateMap<AlgorithmSettings, Parameters>().ReverseMap();
    }
}