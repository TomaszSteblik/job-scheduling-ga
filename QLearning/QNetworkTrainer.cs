using System.Text;
using SchedulingAlgorithmModels.Models;
using TorchSharp;
using static TorchSharp.torch;
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
        for (int i = 0; i < DeepQLearning.workerPrefDaysSize; i++)
        {
            if (i >= workers.Length)
            {
                workerPref[i] = -1;
                continue;
            }
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
        for (int i = 0; i < DeepQLearning.workerQualSize; i++)
        {
            if (i >= workers.Length)
            {
                workerQual[i] = -1;
                continue;
            }
            var worker = workers[i];
            var stringQual = new StringBuilder();
            for (int j = 0; j < DeepQLearning.qualifications; j++)
            {
                stringQual.Append(worker.Qualifications
                    .Any(x => x.Equals(j.ToString(), StringComparison.InvariantCultureIgnoreCase)) ? "1" : "0");
            }
            workerQual[i] = Convert.ToInt64(stringQual.ToString(), 2);            

        }
        var tensor = torch.tensor(workerQual);
        return tensor;
    }

    private Tensor GetWorkerDaysCount(Person[] workers)
    {
        var workerDays = new long[DeepQLearning.workerPrefDaysCountSize];
        for (int i = 0; i < DeepQLearning.workerPrefDaysCountSize; i++)
        {
            if (i >= workers.Length)
            {
                workerDays[i] = -1;
                continue;
            }
            var worker = workers[i];
            workerDays[i] = worker.PreferenceDaysCount;
        }
        var tensor = torch.tensor(workerDays);
        return tensor;
    }
    
    private Tensor GetWorkerPrefMachineTensor(Person[] workers)
    {
        var workerPref = new long[DeepQLearning.workerPrefMachineSize];
        for (int i = 0; i < DeepQLearning.workerPrefMachineSize; i++)
        {
            if (i >= workers.Length)
            {
                workerPref[i] = -1;
                continue;
            }
            var worker = workers[i];
            var stringPref = new StringBuilder();
            for (int j = 0; j < _machines.Length; j++)
            {
                stringPref.Append(worker.PreferredMachineIds.Contains(j) ? "1" : "0");
            }
            workerPref[i] = Convert.ToInt64(stringPref.ToString(), 2);            
        }

        var tensor = torch.tensor(workerPref);
        return tensor;
    }

    private Tensor GetMachineRequirements(Machine[] machines)
    {
        var machinesReq = new long[DeepQLearning.machineReqSize];
        for (int i = 0; i < DeepQLearning.machineReqSize; i++)
        {
            if (i >= machines.Length)
            {
                machinesReq[i] = -1;
                continue;
            }
            var machine = machines[i];
            machinesReq[i] = long.Parse(machine.RequiredQualification);
        }
        var tensor = torch.tensor(machinesReq);
        return tensor;
    }

    void InitializeState()
    {
        State = new float[DeepQLearning.stateSize];
        for (int i = 0; i < State.Length; i++)
        {
            State[i] = -1;
        }
    }
    
    private QAction GetRandomValidAction()
    {
        var current = Utils.ChromosomeFromState(State, 20, _machines, _workers);
        var validActions = ValidActions
            .Where(a => current.Value[a.Day].All(z => z?.Id != a.Worker.Id) 
                        && current.Value[a.Day][a.Machine.Value] is null)
            .ToList();
        
        if (!validActions.Any())
            return null;
            
        int index = Random.Shared.Next(validActions.Count);
        return validActions[index];
    }
    
    private bool IsStateFilled()
    {
        var isFull = State.Count(x => x >= 0) >= _machines.Length * DeepQLearning.days;
        return isFull;
    }
    
    private bool IsGoalReached()
    {
        var isFull = IsStateFilled();
        if (!isFull)
            return false;
        
        var current = Utils.ChromosomeFromState(State, 20, _machines, _workers);

        return current.IsValid(_machines);
    }
    
    private Dictionary<string, ICollection<Person>> CreatePeopleByQualificationIfNull()
    {
        var people = _workers;

        var _peopleByQualification = new Dictionary<string, ICollection<Person>>();
        var qualifications = people.SelectMany(x => x.Qualifications ?? ArraySegment<string>.Empty).Distinct();

        foreach (var qualification in qualifications)
        {
            var qualifiedPeople = people.Where(x =>
                x.Qualifications != null && x.Qualifications.Contains(qualification)).ToArray();
            _peopleByQualification.Add(qualification, qualifiedPeople);
        }

        return _peopleByQualification;
    }
    
    private void FixChromosome(Chromosome offspring)
    {
        var _peopleByQualification = CreatePeopleByQualificationIfNull();
        for (var i = 0; i < offspring.Value.Length; i++)
        {
            for (var j = 0; j < offspring.Value[i].Length; j++)
            {
                var count = offspring.Value[i].Count(x => x.Id == offspring.Value[i][j].Id);
                if (count <= 1)
                    continue;

                var machineRequiredQualification = _machines[j].RequiredQualification;
                
                var qualifiedPeople = _peopleByQualification[machineRequiredQualification];
                var unusedQualifiedPeople = qualifiedPeople
                    .Where(x => !offspring.Value[i].Contains(x))
                    .ToArray();

                offspring.Value[i][j] = unusedQualifiedPeople.Any() ?
                    unusedQualifiedPeople.ElementAt(Random.Shared.Next(unusedQualifiedPeople.Length)) :
                    throw new IndexOutOfRangeException(
                        $"Not enough of qualified workers for {machineRequiredQualification}");

            }
        }
    }
    
    public void TrainQNetwork(DeepQLearning qNetwork, int episodes, int c, int m)
    {
        var optimizer = Adam(qNetwork.parameters(), lr:0.01);
        float gamma = 0.8f;
        var workerPref = GetWorkerPrefTensor(_workers);
        var workerQual = GetWorkerQual(_workers);
        var workerDays = GetWorkerDaysCount(_workers);
        var machineReq = GetMachineRequirements(_machines);
        var workerPrefMachine = GetWorkerPrefMachineTensor(_workers);

        for (int episode = 0; episode < episodes; episode++)
        {
            InitializeState();
            while (!IsGoalReached())
            { 
                torch.Tensor state = torch.tensor(State);
                torch.Tensor qValues = qNetwork.forward(state, workerPref, workerQual, 
                    workerDays, machineReq, workerPrefMachine);
                QAction action = GetRandomValidAction();
                var stateNew = new float[State.Length];
                State.CopyTo(stateNew, 0);

                action.Execute(stateNew);
                float reward = (float)GetRewardBasedOnState(action, State, stateNew);
                State = stateNew; 
                torch.Tensor nextState = torch.tensor(stateNew);
                torch.Tensor nextQValues = qNetwork.forward(nextState, workerPref, workerQual, 
                    workerDays, machineReq, workerPrefMachine);
                float maxNextQ = nextQValues.max().item<float>();

                float targetQ = reward + gamma * maxNextQ;
                Tensor targetQTensor = qValues.clone();

                Tensor actionIndex = torch.tensor(new long[] {  action.Id  }, ScalarType.Int64);

                Tensor targetValue = torch.tensor(new float[] { targetQ  });

                targetQTensor = targetQTensor.scatter(0, actionIndex, targetValue);

                var loss = torch.nn.functional.mse_loss(qValues, targetQTensor);
                
                optimizer.zero_grad();
                loss.backward();
                optimizer.step();
            }
        }
    }
    
    public double GetRewardBasedOnState(QAction action, float[] stateOld, float[] stateNew)
    {
        var current = Utils.ChromosomeFromState(stateOld, 20, _machines, _workers);
        var newChromosome = Utils.ChromosomeFromState(stateNew, 20, _machines, _workers);
        
        var copyState = new float[State.Length];
        State.CopyTo(copyState, 0);
        action.Execute(copyState);
        newChromosome.RecalculateFitness();
        current.RecalculateFitness();
            
        if(AreSame(current, newChromosome))
            return -10;
            
        double result = current.Fitness - newChromosome.Fitness;
        
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
        var workerPrefMachine = GetWorkerPrefMachineTensor(_workers);
        List<int> actionsTaken = new List<int>();

        while (!IsStateFilled())
        {

            Tensor qValues = qNetwork.forward(State, workerPref, workerQual, workerDays, 
                machineReq, workerPrefMachine);
            float[] qValuesArray = qValues.data<float>().ToArray();

            var bestAction = qValuesArray
                .Select((v, i) => (v, i))
                .OrderByDescending(x => x.v)
                .Select(i => i.i)
                .Except(actionsTaken).First();
            
            actionsTaken.Add(bestAction);

            if(bestAction >= ValidActions.Count)
                continue;
            
            var act = ValidActions[(int)bestAction];
            act.Execute(State);
        }
        
        var result = Utils.ChromosomeFromState(State, 20, _machines, _workers);
        if(result.IsValid(_machines) is false)
            FixChromosome(result);

        return result;
    }


}