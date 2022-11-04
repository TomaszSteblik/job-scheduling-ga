using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

public class AddWorkerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IMapper _mapper;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IQualificationsRepository _qualificationsRepository;
    private readonly IMachinesRepository _machinesRepository;

    [Reactive]
    public IEnumerable<QualificationRead> Qualifications { get; set; }
    
    [Reactive]
    public ICollection<MachineRead> Machines { get; set; }

    [Reactive] 
    public IEnumerable<int> Days { get; set; }
    
    [Reactive]
    public AddWorker Worker { get; set; }
    
    public ReactiveCommand<Unit, Unit> AddCommand { get; set; }
    public ViewModelActivator Activator { get; }
    public IEnumerable<int> PreferredDays => Enumerable.Range(0,20);

    public AddWorkerViewModel(IMapper mapper, IPeopleRepository peopleRepository,
        IQualificationsRepository qualificationsRepository, IMachinesRepository machinesRepository)
    {
        _mapper = mapper;
        _peopleRepository = peopleRepository;
        _qualificationsRepository = qualificationsRepository;
        _machinesRepository = machinesRepository;
        Worker = new AddWorker
        {
            Qualifications = new ObservableCollection<QualificationRead>(),
            PreferredMachines = new ObservableCollection<MachineRead>(),
            PreferredDays = new ObservableCollection<int>()
        };
        Activator = new ViewModelActivator();
        AddCommand = ReactiveCommand.CreateFromTask(AddPerson);
        AddCommand.LogExceptions();
        this.WhenActivated(WhenActivated);
    }

    private async void WhenActivated(CompositeDisposable disposables)
    {
        Qualifications = await _qualificationsRepository.GetQualifications();
        Machines = await _machinesRepository.GetMachines();
        Days = Enumerable.Range(1,20);
        Disposable.Create(() =>
            {
                /* handle deactivation */
            })
            .DisposeWith(disposables);
    }



    private async Task AddPerson() => await _peopleRepository.AddPerson(_mapper.Map<PersonWrite>(Worker));
}