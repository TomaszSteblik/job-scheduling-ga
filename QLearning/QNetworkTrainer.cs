using System.Text;
using SchedulingAlgorithmModels.Models;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.optim;

namespace QLearning;

public class QNetworkTrainer
{
    public float[] State { get; set; }
    private readonly Machine[] _machines;
    private readonly Person[] _workers;
    public List<QAction> ValidActions { get; set; }
    
    public QNetworkTrainer(Machine[] machines, Person[] people)
    {
        _workers = people;
        _machines = machines;
        
        var actionCounter = 0;
        ValidActions = new List<QAction>();
        for (int machineIndex = 0; machineIndex < machines.Length; machineIndex++)
        {
            var machine = machines[machineIndex];
            for (int workerIndex = 0; workerIndex < people.Length; workerIndex++)
            {
                var worker = people[workerIndex];
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
    }
    
    private Tensor GetWorkerPrefTensor(Person[] workers)
    {
        var workerPref = new long[DeepQLearning.workerPrefDaysSize];
        //Tensor workerPref = torch.zeros(new long[] { DeepQLearning.workerPrefDaysSize });
        for (int i = 0; i < workers.Length; i++)
        {
            var worker = workers[i];
            var stringPref = new StringBuilder();
            for (int j = 0; j < DeepQLearning.days; j++)
            {
                stringPref.Append(worker.PreferredDays.Contains(j) ? "1" : "0");
            }
            workerPref[i] = Convert.ToInt64(stringPref.ToString(), 2);            
        }

        var tensor = torch.tensor(workerPref);
        return tensor;
    }

    private Tensor GetWorkerQual(Person[] workers)
    {
        var workerQual = new long[DeepQLearning.workerQualSize];
        for (int i = 0; i < workers.Length; i++)
        {
            var worker = workers[i];
            var stringQual = new StringBuilder();
            for (int j = 0; j < DeepQLearning.qualifications; j++)
            {
                stringQual.Append(worker.Qualifications.Any(x => x.Equals(j.ToString(), StringComparison.InvariantCultureIgnoreCase)) ? "1" : "0");
            }
            workerQual[i] = Convert.ToInt64(stringQual.ToString(), 2);            

        }
        var tensor = torch.tensor(workerQual);
        return tensor;
    }

    private Tensor GetWorkerDaysCount(Person[] workers)
    {
        var workerDays = new long[DeepQLearning.workerPrefDaysCountSize];
        for (int i = 0; i < workers.Length; i++)
        {
            var worker = workers[i];
            workerDays[i] = worker.PreferenceDaysCount;
        }
        var tensor = torch.tensor(workerDays);
        return tensor;
    }

    private Tensor GetMachineRequirements(Machine[] machines)
    {
        var machinesReq = new long[DeepQLearning.machineReqSize];
        //Tensor workerPref = torch.zeros(new long[] { DeepQLearning.workerPrefDaysSize });
        for (int i = 0; i < machines.Length; i++)
        {
            var machine = machines[i];
            machinesReq[i] = long.Parse(machine.RequiredQualification);
        }
        var tensor = torch.tensor(machinesReq);
        return tensor;
    }

    void InitializeState()
    {
        var c = new Chromosome(20, _machines.Length);
        State = new float[DeepQLearning.stateSize];
        for (int i = 0; i < State.Length; i++)
        {
            State[i] = -1;
        }
    }
    
    private QAction GetRandomValidActionZ()
    {
        var current = Utils.ChromosomeFromState(State, 20, _machines, _workers);
        var validActions = ValidActions
            .Where(a => current.Value[a.Day].All(z => z?.Id != a.Worker.Id))
            .ToList();

        //Console.WriteLine($" DUPA {validActions.Count()}");
            
        if (!validActions.Any())
            return null;
            
        int index = Random.Shared.Next(validActions.Count);
        return validActions[index];
    }
    
    private bool IsGoalReached()
    {
        return State.Count(x => x >= 0) >= _machines.Length * DeepQLearning.days;
    }
    
    public void TrainQNetwork(DeepQLearning qNetwork, int episodes)
    {
        
        var optimizer = Adam(qNetwork.parameters(), lr:0.01);
        float gamma = 0.8f;
        int numActions = DeepQLearning.actionSize;
        Random rand = new Random();

        var workerPref = GetWorkerPrefTensor(_workers);
        var workerQual = GetWorkerQual(_workers);
        var workerDays = GetWorkerDaysCount(_workers);
        var machineReq = GetMachineRequirements(_machines);

        for (int episode = 0; episode < episodes; episode++)
        {
            InitializeState();
            // Sample a random state
            while (!IsGoalReached())
            { 

            torch.Tensor state = torch.tensor(ConvertStateToFloatArray(State));
            
            // Get Q-values for the current state
             torch.Tensor qValues = qNetwork.forward(state, workerPref, workerQual, workerDays, machineReq);
            //var tensor = torch.rand(new long[] { 10, 10, 10, 10, 10 });
            //var qValues = qNetwork.model.call(tensor);
            // Select action (ε-greedy)
            QAction action = GetRandomValidActionZ();
            var stateNew = new float[State.Length];
            State.CopyTo(stateNew, 0);

            action.Execute(stateNew);
            // Perform action
            float reward = (float)GetRewardBasedOnState(action, State, stateNew);
            //if(reward >=0)
            State = stateNew; 
            // Sample next state (simulated for training)
            torch.Tensor nextState = torch.tensor(ConvertStateToFloatArray(stateNew));
            // Compute max Q-value for the next state
            torch.Tensor nextQValues = qNetwork.forward(nextState, workerPref, workerQual, workerDays, machineReq);
            float maxNextQ = nextQValues.max().item<float>();

            // Q-learning update rule
            float targetQ = reward + gamma * maxNextQ;
            Tensor targetQTensor = qValues.clone();

// Create an index tensor of shape (1,1) with the action index.
// If 'action' is the selected action for the single sample:
            Tensor actionIndex = torch.tensor(new long[] {  action.Id  }, torch.ScalarType.Int64);

// Create a tensor for the new Q-value, also with shape (1,1)
            Tensor targetValue = torch.tensor(new float[] { targetQ  });

// Scatter along dimension 1 (columns)
            targetQTensor = targetQTensor.scatter(0, actionIndex, targetValue);

            // Compute loss (Mean Squared Error)
            var loss = torch.nn.functional.mse_loss(qValues, targetQTensor);
            
            // Backpropagation
            optimizer.zero_grad();
            loss.backward();
            optimizer.step();

            Console.WriteLine($"Episode {episode + 1}, Loss: {loss.item<float>()} Action: {action.Id} Reward: {reward} Q-Value: {qValues[0].item<float>()} NotNull: {State.Count(x => x >= 0)}");

            }

            var current = Utils.ChromosomeFromState(State, 20, _machines, _workers);
            current.RecalculateFitness();
            Console.WriteLine("Episode {0}, {1}", episode + 1, current.IsValid(_machines));
        }

        bool AreTheSame(int?[] stateOld, int?[] stateNew)
        {
            for (int i = 0; i < stateNew.Length; i++)
            {
                if(stateNew[i] != stateOld[i])
                    return false;
            }

            return true;
        }
    }
    
    public double GetRewardBasedOnState(QAction action, float[] stateOld, float[] stateNew)
    {
        var current = Utils.ChromosomeFromState(stateOld, 20, _machines, _workers);
        var newChromosome = Utils.ChromosomeFromState(stateNew, 20, _machines, _workers);

        if (current.Value[action.Day].Any(z => z?.Id == action.Worker.Id))
            return -99;

        var copyState = new float[State.Length];
        State.CopyTo(copyState, 0);
        action.Execute(copyState);
        newChromosome.RecalculateFitness();
        current.RecalculateFitness();
            
        if(AreSame(current, newChromosome))
            return -100;
            
        double result = current.Fitness - newChromosome.Fitness;
        if (result == 0) result = -1;
        return result;
    }
    
    private bool AreSame(Chromosome chromosome1, Chromosome chromosome2)
    {
        bool result = true;

        for (int j = 0; j < chromosome1.Value.Length; j++)
        {
            for (int k = 0; k < chromosome1.Value[j].Length; k++)
            {
                if (chromosome1.Value[j][k] != chromosome2.Value[j][k])
                    return false;
            }
        }
            
        return true;
    }
    
    public Chromosome GenerateSolution(DeepQLearning qNetwork)
    {
        InitializeState();
        var workerPref = GetWorkerPrefTensor(_workers);
        var workerQual = GetWorkerQual(_workers);
        var workerDays = GetWorkerDaysCount(_workers);
        var machineReq = GetMachineRequirements(_machines);
        // List to store the chosen actions as part of the solution
        List<int> actionsTaken = new List<int>();

        // Set a maximum number of steps to prevent infinite loops
        int maxSteps = 1000;
        int currentStep = 0;

        // Loop until a terminal condition is met (this depends on your problem/environment)
        while (!IsGoalReached())
        {
            Console.WriteLine($"Step {currentStep + 1}: NotNull: {State.Count(x => x >= 0)}");

            // Use the trained model to compute Q-values
            Tensor qValues = qNetwork.forward(ConvertStateToFloatArray(State), workerPref, workerQual, workerDays, machineReq);
            float[] qValuesArray = qValues.data<float>().ToArray();

            var bestAction = qValuesArray.Select((v, i) => (v, i)).OrderByDescending(x => x.v).Select(i => i.i)
                .Except(actionsTaken).First();
            
            // Choose the best action (greedy selection, no exploration)
            //var bestAction = qValues.argmax(0).item<Int64>();
            Console.WriteLine($"Step {currentStep + 1}: Best action = {bestAction} NotNull: {State.Count(x => x >= 0)}");

            actionsTaken.Add((int) bestAction);

            if(bestAction >= ValidActions.Count)
                continue;
            var act = ValidActions[(int)bestAction];
            act.Execute(State);

            

            // Optionally, check if the state is terminal and break the loop if it is.
            // For demonstration, we break after maxSteps.
            currentStep++;
        }

        // Output the generated solution (sequence of actions)
        Console.WriteLine("Solution (actions taken): " + string.Join(" -> ", actionsTaken));
        
        return Utils.ChromosomeFromState(State, 20, _machines, _workers);
    }
    
    private float[] ConvertStateToFloatArray(float[] state)
    {
        // float[] input = new float[DeepQLearning.stateSize];
        // for (int i = 0; i < state.Length; i++)
        // {
        //     if (!state[i].HasValue)
        //         continue;
        //     var (day, worker) = Utils.PairingFunction(i);
        //     var machine = state[i].Value;
        //     input[day * _machines.Length + machine] = worker;
        // }
            
        return state;
    }


}