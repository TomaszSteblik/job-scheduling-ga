using System.Reactive;
using AutoMapper;
using Data.Dtos.Write;
using Data.Repositories;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Models;

namespace Scheduling.ViewModels;

public class AddWorkerViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IPeopleRepository _peopleRepository;

    [Reactive]
    public Worker Worker { get; set; }
    
    public ReactiveCommand<Unit, Unit> AddCommand { get; set; }

    public AddWorkerViewModel(IMapper mapper, IPeopleRepository peopleRepository)
    {
        _mapper = mapper;
        _peopleRepository = peopleRepository;
        Worker = new Worker();
        AddCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await _peopleRepository.AddPerson(_mapper.Map<PersonWrite>(Worker));
        });
    }
}