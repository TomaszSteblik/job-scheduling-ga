using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;

namespace Data.MapperProfiles;

internal class MachineProfile : Profile
{
    public MachineProfile()
    {
        CreateMap<Machine, MachineRead>().ReverseMap();
        CreateMap<Machine, MachineWrite>().ReverseMap();
        CreateMap<Machine, MachineUpdate>().ReverseMap();
    }
}