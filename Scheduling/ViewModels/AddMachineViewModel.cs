using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dtos.Read;
using Data.Dtos.Write;
using Data.Repositories;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Helpers;
using Scheduling.Models;

namespace Scheduling.ViewModels;

public class AddMachineViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IMapper _mapper;
    private readonly IMachinesRepository _machinesRepository;
    private readonly IQualificationsRepository _qualificationsRepository;
    public ViewModelActivator Activator { get; }

    [Reactive]
    public AddMachine Machine { get; set; }

    [Reactive]
    public IEnumerable<QualificationRead> Qualifications { get; set; }

    public ReactiveCommand<Unit, Unit> AddCommand { get; set; }

    public AddMachineViewModel(IMapper mapper, IMachinesRepository machinesRepository, IQualificationsRepository qualificationsRepository)
    {
        _mapper = mapper;
        _machinesRepository = machinesRepository;
        _qualificationsRepository = qualificationsRepository;
        Qualifications = new List<QualificationRead>();
        Machine = new AddMachine();
        Activator = new ViewModelActivator();
        AddCommand = ReactiveCommand.CreateFromTask(AddMachine);
        AddCommand.LogExceptions();
        this.WhenActivated(ActivationHandler);
    }

    private async Task AddMachine()
    {
        var machineWrite = _mapper.Map<MachineWrite>(Machine);
        await _machinesRepository.AddMachine(machineWrite);
    }

    async void ActivationHandler(CompositeDisposable disposables)
    {
        Qualifications = await _qualificationsRepository.GetQualifications();
        Disposable.Create(() =>
            {
                /* handle deactivation */
            })
            .DisposeWith(disposables);
    }
}