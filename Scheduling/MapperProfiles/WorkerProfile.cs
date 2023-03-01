using System.Linq;
using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using GeneticAlgorithm.Models;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class WorkerProfile : Profile
{
    public WorkerProfile()
    {
        CreateMap<Worker, PersonRead>().ReverseMap();
        CreateMap<Worker, PersonWrite>().ReverseMap();
        CreateMap<Worker, PersonUpdate>().ReverseMap();
        CreateMap<Worker, Person>()
            .ForMember(dest => dest.Name,
                opt =>
                    opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Surname,
                opt =>
                    opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.PreferredMachineIds,
                opt =>
                    opt.MapFrom(src => src.PreferredMachines.Select((item, index) => index)))
            .ForMember(dest => dest.PreferenceDaysCount,
                opt =>
                    opt.MapFrom(src=> src.PreferredDays.Count))
            .ReverseMap();
        CreateMap<AddWorker, PersonWrite>();
    }
}