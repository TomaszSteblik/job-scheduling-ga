using System.Collections.Immutable;
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
        CreateMap<Person, PersonRead>()
            .ForMember(x => x.PreferredDays, opt =>
                opt.MapFrom(z => (z.PreferredDays ?? Array.Empty<Day>()).Select(day => day.DayOfSchedule)))
            .ReverseMap();
        CreateMap<Person, PersonWrite>()
            .ForMember(x => x.PreferredDays, opt =>
                opt.MapFrom(z => (z.PreferredDays ?? Array.Empty<Day>()).Select(day => day.DayOfSchedule)));
        CreateMap<Person, PersonUpdate>()
            .ForMember(x => x.PreferredDays, opt =>
                opt.MapFrom(z => (z.PreferredDays ?? Array.Empty<Day>()).Select(day => day.DayOfSchedule)))
            .ReverseMap();
        CreateMap<PersonWrite, Person>()
            .ForMember(x => x.PreferredDays, opt =>
                opt.MapFrom(src => src.PreferredDays != null ? src.PreferredDays.Select(i => new Day()
                {
                    DayOfSchedule = i
                }) : ImmutableArray<Day>.Empty));

    }
}