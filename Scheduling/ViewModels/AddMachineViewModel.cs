using System.Reactive;
using AutoMapper;
using Data.Dtos.Write;
using Data.Repositories;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Models;

namespace Scheduling.ViewModels;

public class AddMachineViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IMachinesRepository _machinesRepository;

    [Reactive]
    public Machine Machine { get; set; }

    public ReactiveCommand<Unit, Unit> AddCommand { get; set; }

    public AddMachineViewModel(IMapper mapper, IMachinesRepository machinesRepository)
    {
        _mapper = mapper;
        _machinesRepository = machinesRepository;
        Machine = new Machine();
        AddCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await machinesRepository.AddMachine(_mapper.Map<MachineWrite>(Machine));
        });
    }
}