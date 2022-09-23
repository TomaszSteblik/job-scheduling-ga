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
        CreateMap<Worker, Person>().ReverseMap();
        CreateMap<AddWorker, PersonWrite>();
    }
}