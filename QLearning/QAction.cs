using SchedulingAlgorithmModels.Models;

namespace QLearning;

public class QAction(int ID, int? Machine, Person Worker, int Day)
{
    public int Id { get; } = ID;
    public Person Worker { get; } = Worker;
    public int Day { get; } = Day;
    public int? Machine { get; set; } = Machine;


    public void Execute(int?[] State)
    {
        State[Utils.PairingFunction(Day, Worker.Id)] = Machine;
    }
    
    public void Execute(float[] State)
    {
        State[Day * DeepQLearning.machines + Machine.Value] = Worker.Id;
    }
}