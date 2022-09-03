using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;

namespace Data.MapperProfiles;

internal class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonRead>().ReverseMap();
        CreateMap<Person, PersonWrite>().ReverseMap();
        CreateMap<Person, PersonUpdate>().ReverseMap();
    }
}