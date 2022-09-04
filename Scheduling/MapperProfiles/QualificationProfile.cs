using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class QualificationProfile : Profile
{
    public QualificationProfile()
    {
        CreateMap<Qualification, GeneticAlgorithm.Models.Qualification>().ReverseMap();
        CreateMap<Qualification, QualificationUpdate>().ReverseMap();
        CreateMap<Qualification, QualificationRead>().ReverseMap();
        CreateMap<Qualification, QualificationWrite>().ReverseMap();
    }
}