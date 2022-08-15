using System.Reactive.Disposables;
using ReactiveUI;

namespace Scheduling.ViewModels;

public class AlgorithmParametersViewModel : ViewModelBase, IActivatableViewModel
{
    public string Name => "GA PARAMS";

    public AlgorithmParametersViewModel()
    {
        Activator = new ViewModelActivator();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            /* handle activation */
            Disposable
                .Create(() => { /* handle deactivation */ })
                .DisposeWith(disposables);
        });
    }

    public ViewModelActivator Activator { get; }
}