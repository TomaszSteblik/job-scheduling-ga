using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;

namespace Data.MapperProfiles;

internal class QualificationProfile : Profile
{
    public QualificationProfile()
    {
        CreateMap<Qualification, QualificationRead>().ReverseMap();
        CreateMap<Qualification, QualificationWrite>().ReverseMap();
        CreateMap<Qualification, PropertyQualificationWrite>().ReverseMap();
        CreateMap<Qualification, QualificationUpdate>().ReverseMap();
        CreateMap<QualificationRead, PropertyQualificationWrite>().ReverseMap();
    }
}