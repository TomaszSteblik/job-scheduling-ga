using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

    public ViewModelActivator Activator { get; }
    [Reactive]
    public IEnumerable<Worker>? AvailableWorkers { get; set; }
    [Reactive]
    public IEnumerable<Machine>? AvailableMachines { get; set; }

    public ObservableCollection<Worker>? SelectedWorkers { get; set; }

    public ObservableCollection<Machine>? SelectedMachines { get; set; }

    public SelectDataViewModel(IDataRepository dataRepository, ISelectedDataRepository selectedDataRepository)
    {
        _dataRepository = dataRepository;
        _selectedDataRepository = selectedDataRepository;
        Activator = new ViewModelActivator();
        SelectedWorkers = new ObservableCollection<Worker>();
        SelectedMachines = new ObservableCollection<Machine>();
        

        this.WhenActivated(async disposables =>
        {
            await LoadData();
            
            /* handle activation */
            Disposable
                .Create(() =>
                {
                    _selectedDataRepository.SetMachines(SelectedMachines);
                    _selectedDataRepository.SetWorkers(SelectedWorkers);
                })
                .DisposeWith(disposables);
        });
    }

    private async Task LoadData()
    {
        AvailableMachines = await _dataRepository.GetMachines();
        AvailableWorkers = await _dataRepository.GetWorkers();
    }

}