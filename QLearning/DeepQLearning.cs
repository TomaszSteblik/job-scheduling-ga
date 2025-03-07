
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace QLearning;

public class DeepQLearning : torch.nn.Module
{
    public Sequential model;
    public double[] State { get; set; }
    public const int workers = 100;
    public const int machines = 10;
    public const int days = 20;
    public const int qualifications = 10;

    public static int stateSize = machines * days;
    public const int workerPrefDaysSize = days;
    public const int workerQualSize = workers;
    public const int workerPrefDaysCountSize = workers;
    public const int machineReqSize = machines;
    public const int actionSize = workers*machines*days;

    public DeepQLearning(): base("QNetwork")
    {
        //harmonogram, maszyny, pracownicy, preferencje maszyn, preferencje dni, preferencje ilosci dni, dzien, pracownik
        int totalInputSize = stateSize + workerPrefDaysSize + (workerQualSize + workerPrefDaysCountSize) + machineReqSize;
        
        model = Sequential(
            Linear(totalInputSize, 64), 
            ReLU(),
            Linear(64, 32), 
            ReLU(),
            Linear(32, actionSize)  // Output Q-values for each action
        );

        this.RegisterComponents();
    }
    
    public Tensor forward(Tensor state, Tensor workerPref, Tensor workerQual, Tensor workerDays, Tensor machineReq)
    {
        Tensor input = torch.cat(new Tensor[] { state.flatten(), workerPref.flatten(), workerQual.flatten(), workerDays.flatten(), machineReq.flatten() });
        return model.call(input);
    }
    
}