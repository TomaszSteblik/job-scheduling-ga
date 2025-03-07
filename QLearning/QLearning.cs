using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Microsoft.VisualBasic;
using SchedulingAlgorithmModels.Models;

namespace QLearning;

public class QLearning
{
    private ActivationNetwork qNetwork;
    private BackPropagationLearning teacher;
    //      DAYS 0 1 2 3 4 ...
    //MACHINE
    //      0    W1 W2 W1 
    //      1
    //      2
    //      ...
    //public int[][] State { get; set; }
    //worker set at given day and machine
    public int?[] StateNew { get; set; }
    //public double[,] QTable { get; set; }
    public Dictionary<(string, int), double> NewQTalbe { get; set; }
    public List<QAction> ValidActions { get; set; }
    public IReadOnlyList<QAction> OriginalValidActions { get; set; }

    private readonly Machine[] _machines;
    private readonly Person[] _workers;

    public QLearning(Machine[] machines, Person[] workers)
    {
        _workers = workers;
        _machines = machines;
        StateNew = new int?[Utils.PairingFunction(20, workers.Length)];
        NewQTalbe = new Dictionary<(string, int), double>();
        // State = new int[][20];
        // for (int i = 0; i < 20; i++)
        // {
        //     State[i] = new int[machines.Length];
        //     for (int j = 0; j < machines.Length; j++)
        //     {
        //         State[i][j] = -1;
        //     }
        // }
        //ValidActions = new QAction[20 * machines.Length * workers.Length];
        var actionCounter = 0;
        ValidActions = new List<QAction>();
        for (int machineIndex = 0; machineIndex < machines.Length; machineIndex++)
        {
            var machine = machines[machineIndex];
            for (int workerIndex = 0; workerIndex < workers.Length; workerIndex++)
            {
                var worker = workers[workerIndex];
                for (int day = 0; day < 20; day++)
                {
                    if (worker.Qualifications.Any(x =>
                            x.Equals(machine.RequiredQualification, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        ValidActions.Add(new QAction(actionCounter++, machineIndex, worker, day));
                    }
                }
            }
        }
        OriginalValidActions = new List<QAction>(ValidActions);
        qNetwork = new ActivationNetwork(new SigmoidFunction(), StateNew.Length, 20, ValidActions.Count);
        teacher = new BackPropagationLearning(qNetwork);
        //QTable = new double[OriginalValidActions.Count, SzudzikPairingFunction(20, machines.Length)];
    }
    
    // public Chromosome Run()
    // {
    //         
    //     StateNew = new int?[PairingFunction(20, _workers.Length)];
    //     List<int> actions = new List<int>();
    //     while (true)
    //     {
    //         var currentState = Strings.Join(StateNew.Select(x=> x.ToString()).ToArray(), ",");
    //         int actionIndex = NewQTalbe.Where(x => x.Key.Item1 == currentState).MaxBy(x => x.Value).Key.Item2;
    //         var action = ValidActions.First(x=> x.Id == actionIndex);
    //         action.Execute(StateNew);
    //         if (!IsGoalReached()) continue;
    //         
    //         Console.WriteLine("Goal reached");
    //         var result = new Chromosome(20, _machines.Length);
    //         for (var i = 0; i < StateNew.Length; i++)
    //         {
    //             if(!StateNew[i].HasValue)
    //                 continue;
    //             var (day, worker) = PairingFunction(i);
    //             var machine = StateNew[i].Value;
    //
    //             result.Value[day][machine] = Enumerable.First(_workers, x => x.Id == worker);
    //
    //             
    //         }
    //
    //         return result;
    //     }
    // }
    
    public Chromosome Run()
    {
        StateNew = new int?[Utils.PairingFunction(20, _workers.Length)];

        while (true)
        {
            // Convert state to input format for the neural network
            double[] currentState = StateNew.Select(x => x.HasValue ? (double)x.Value : 0.0).ToArray();

            // Predict Q-values for the current state
            double[] qValues = qNetwork.Compute(currentState);

            // Choose the best action
            int bestActionIndex = qValues.ArgMax();
            QAction bestAction = ValidActions.First(x => x.Id == bestActionIndex);

            // Execute the chosen action
            bestAction.Execute(StateNew);

            // Check if goal state is reached
            if (!IsGoalReached()) 
                continue;

            Console.WriteLine("Goal reached");
            var result = new Chromosome(20, _machines.Length);

            for (var i = 0; i < StateNew.Length; i++)
            {
                if (!StateNew[i].HasValue)
                    continue;

                var (day, worker) = Utils.PairingFunction(i);
                var machine = StateNew[i].Value;

                result.Value[day][machine] = _workers.FirstOrDefault(x => x.Id == worker);
            }

            return result;
        }
    }

    
    public void TrainAgent(int numberOfIterations)
    {
        for(int i = 0; i < numberOfIterations; i++)
        {
            InitializeEpisode();
            Console.WriteLine("Episode: " + i);
        }
    }
    
    private void InitializeEpisode()
    {
        //State = new int[][20];
        StateNew = new int?[Utils.PairingFunction(20, _workers.Length)];
        // for (int i = 0; i < 20; i++)
        // {
        //     State[i] = new int[_machines.Length];
        //     for (int j = 0; j < _machines.Length; j++)
        //     {
        //         State[i][j] = -1;
        //     }
        // }
        while (true)
        {
            TakeAction();
            if (IsGoalReached())
                break;
        }
    }

    private bool IsGoalReached()
    {
        var result = new Chromosome(20, _machines.Length);
        for (var i = 0; i < StateNew.Length; i++)
        {
            if(!StateNew[i].HasValue)
                continue;
            var (day, worker) = Utils.PairingFunction(i);
            var machine = StateNew[i].Value;

            result.Value[day][machine] = Enumerable.First(_workers, x => x.Id == worker);

                
        }

        // if (result.Value.SelectMany(x => x).Any(x => x is null))
        //     return false;
        //result.RecalculateFitness();
        // if (result.Fitness >= 100)
        //     return false;
        return !result.Value.SelectMany(x => x).Any(x => x is null);

        // var machinesOccupied = StateNew.Where(x => x.HasValue).Select(x => x.Value).Count();
        // return machinesOccupied == _machines.Length * 20;
    }
    
    // private void TakeAction()
    // {
    //     var action = GetRandomValidAction();
    //     var currentState = Strings.Join(StateNew.Select(x=> x.ToString()).ToArray(), ",");
    //
    //     double saReward = GetRewardBasedOnState(action);
    //     if(NewQTalbe.All(x => x.Key.Item2 != action.Id))
    //         NewQTalbe.Add((currentState, action.Id), 0);
    //     double nsReward = NewQTalbe.Where(x => x.Key.Item2 == action.Id).Select(z => z.Value).Max();
    //     double qCurrentState = saReward + (0.8 * nsReward);
    //     NewQTalbe[(currentState, action.Id)] = qCurrentState;
    //     action.Execute(StateNew);
    // }
    
    private void TakeAction()
    {
        var action = GetRandomValidAction();
        var currentState = StateNew.Select(x => x.HasValue ? (double)x.Value : -1.0).ToArray();

        // Predict Q-values for the current state
        double[] qValues = qNetwork.Compute(currentState);

        // Get the best action
        int bestActionIndex = qValues.ArgMax();
        QAction bestAction = ValidActions.First(x => x.Id == bestActionIndex);

        // Calculate the reward
        action.Execute(StateNew);
        double saReward = GetRewardBasedOnState(action);

        // Estimate future Q-value
        double[] nextQValues = qNetwork.Compute(currentState);
        double maxFutureQ = Enumerable.Max(nextQValues);

        // Q-learning formula
        double qCurrentState = saReward + (0.8 * maxFutureQ);

        // Train the network using backpropagation
        double[] targetQValues = qValues;
        targetQValues[bestActionIndex] = qCurrentState;
        teacher.Run(currentState, targetQValues);

        // Execute the action
    }

    
    public double GetRewardBasedOnState(QAction action)
    {
        // var currentlyWorkedDays = StateNew.Where((x, i) => PairingFunction(i).Item2 == action.Worker.Id && x.HasValue).Select((x,i) => PairingFunction(i).Item1);
        // var numberOfDaysWorked = currentlyWorkedDays.Count();
        // var reward = 0;
        //
        // reward += action.Worker.PreferredDays.Contains(action.Day) ? 10 : 0;
        // reward += action.Worker.PreferredMachineIds.Contains(action.Machine) ? 10 : 0;
        // reward += ((numberOfDaysWorked + 1) + action.Worker.PreferenceDaysCount) % action.Worker.PreferenceDaysCount;
        var copyState = new int?[StateNew.Length];
        StateNew.CopyTo(copyState, 0);
        action.Execute(copyState);
        var current = new Chromosome(20, _machines.Length);
        for (var i = 0; i < StateNew.Length; i++)
        {
            if(!StateNew[i].HasValue)
                continue;
            var (day, worker) = Utils.PairingFunction(i);
            var machine = StateNew[i].Value;

            current.Value[day][machine] = Enumerable.First(_workers, x => x.Id == worker);

                
        }
        
        var newChromosome = new Chromosome(20, _machines.Length);
        for (var i = 0; i < copyState.Length; i++)
        {
            if(!copyState[i].HasValue)
                continue;
            var (day, worker) = Utils.PairingFunction(i);
            var machine = copyState[i].Value;

            newChromosome.Value[day][machine] = Enumerable.First(_workers, x => x.Id == worker);

                
        }

        try
        {
            newChromosome.RecalculateFitness();
            current.RecalculateFitness();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return newChromosome.Fitness - current.Fitness;
    }

    //USUWAC ZROBIONE AKCJE I NIE POZWALAC NA NADPISANIE
    private QAction GetRandomValidAction()
    {
        while (true)
        {
            var random = new Random();

            var correctActions = ValidActions.Where(a => StateNew[Utils.PairingFunction(a.Day, a.Worker.Id)] is null);
            if (!correctActions.Any())
                return null;
            
            var index = random.Next(0, correctActions.Count());
            var action = correctActions.ElementAt(index);
            //return State[action.Day].Contains(action.Worker.Id) ? GetRandomValidAction() : action;
            //if (StateNew[PairingFunction(action.Day, action.Worker.Id)] is not null) continue;
            return action;
            break;
        }
    }
}