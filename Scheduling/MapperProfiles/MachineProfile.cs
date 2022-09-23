using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class MachineProfile : Profile
{
    public MachineProfile()
    {
        CreateMap<Machine, GeneticAlgorithm.Models.Machine>().ReverseMap();
        CreateMap<Machine, MachineRead>().ReverseMap();
        CreateMap<Machine, MachineWrite>().ReverseMap();
        CreateMap<Machine, MachineUpdate>().ReverseMap();
        CreateMap<AddMachine, MachineWrite>().ReverseMap();
    }
}