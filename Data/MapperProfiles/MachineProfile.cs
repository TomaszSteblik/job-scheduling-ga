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
        CreateMap<Machine, MachineWrite>();
        CreateMap<Machine, MachineUpdate>().ReverseMap();
        CreateMap<Machine, PropertyMachineWrite>().ReverseMap();
        CreateMap<MachineRead, PropertyMachineWrite>().ReverseMap();
        CreateMap<MachineWrite, Machine>()
            .ForMember(x => x.QualificationId, src => src.MapFrom(z => z.RequiredQualification.Id));

    }
}