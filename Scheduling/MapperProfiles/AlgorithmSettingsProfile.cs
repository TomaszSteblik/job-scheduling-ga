using AutoMapper;
using Scheduling.Models;
using SchedulingAlgorithmModels.Models;

namespace Scheduling.MapperProfiles;

public class AlgorithmSettingsProfile : Profile
{
    public AlgorithmSettingsProfile()
    {
        CreateMap<AlgorithmSettings, Parameters>().ReverseMap();
    }
}