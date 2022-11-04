using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Models;
using Scheduling.Repositories;

namespace Scheduling.ViewModels;

public class SelectDataViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IDataRepository _dataRepository;
    private readonly ISelectedDataRepository _selectedDataRepository;
    private ICollection<Worker>? _selectedWorkers;
    private ICollection<Machine>? _selectedMachines;

    public ViewModelActivator Activator { get; }
    [Reactive]
    public IEnumerable<Worker>? AvailableWorkers { get; set; }
    [Reactive]
    public IEnumerable<Machine>? AvailableMachines { get; set; }

    public ICollection<Worker>? SelectedWorkers
    {
        get => _selectedWorkers;
        set
        {
            _selectedWorkers = value;
            if (_selectedWorkers is not null)
                _selectedDataRepository.SetWorkers(_selectedWorkers);
        }
    }

    public ICollection<Machine>? SelectedMachines
    {
        get => _selectedMachines;
        set
        {
            _selectedMachines = value;
            if(_selectedMachines is not null)
                _selectedDataRepository.SetMachines(_selectedMachines);
        }
    }

    public SelectDataViewModel(IDataRepository dataRepository, ISelectedDataRepository selectedDataRepository)
    {
        _dataRepository = dataRepository;
        _selectedDataRepository = selectedDataRepository;
        Activator = new ViewModelActivator();
        SelectedWorkers = new List<Worker>();
        SelectedMachines = new List<Machine>();
        
        this.WhenActivated(async disposables =>
        {
            await LoadData();
            
            /* handle activation */
            Disposable
                .Create(() => { /* handle deactivation */ })
                .DisposeWith(disposables);
        });
    }

    private async Task LoadData()
    {
        AvailableMachines = await _dataRepository.GetMachines();
        AvailableWorkers = await _dataRepository.GetWorkers();
        
        SelectedMachines = new List<Machine>();
        foreach (var machine in _selectedDataRepository.GetMachines())
        {
            SelectedMachines.Add(AvailableMachines.First(x => x == machine));
        }

        SelectedWorkers = new List<Worker>();
        foreach (var worker in _selectedDataRepository.GetWorkers())
        {
            SelectedWorkers.Add(AvailableWorkers.First(x=> x== worker));
        }
        
    }

}