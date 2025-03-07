using System;
using System.Collections.Generic;
using System.Linq;
using Accord;
using Accord.Math;
using Microsoft.VisualBasic;
using SchedulingAlgorithmModels.Models;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using static QLearning.Utils;

namespace QLearning
{
    public class SimpleQLearning
    {
        public int?[] State { get; set; }
        // Removed QTable, now using a neural network to approximate Q-values.
        public List<QAction> ValidActions { get; set; }
        
        private readonly Machine[] _machines;
        private readonly Person[] _workers;
        private Chromosome? _currentChromosome = null;
        
        // Neural network and training algorithm fields
        private readonly ActivationNetwork network;
        private readonly BackPropagationLearning teacher;
        private readonly double gamma = 0.8;
        
        public SimpleQLearning(Machine[] machines, Person[] workers)
        {
            _workers = workers;
            _machines = machines;
            State = new int?[PairingFunction(20, workers.Length)];
            
            int actionCounter = 0;
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
                        //ValidActions.Add(new QAction(actionCounter++, null, worker, day));

                    }
                }
            }
            
            // Initialize the neural network:
            int stateLength = _machines.Length * 20;
            int hiddenNeurons = 20; // Adjust as needed.
            network = new ActivationNetwork(new SigmoidFunction(2), stateLength, hiddenNeurons, ValidActions.Count);
// Randomize the weights (optional)
              var lastLayer = network.Layers[^1] as ActivationLayer;
              lastLayer.SetActivationFunction(new LinearFunction(2, new DoubleRange(-10000, 10000)));
            new NguyenWidrow(network).Randomize();

            teacher = new BackPropagationLearning(network)
            {
                LearningRate = 0.1,
                Momentum = 0.9
            };

            
        }
        
        public Chromosome Run()
        {
            InitializeChromosome();
            var usedActions = new List<int>();
            while(!IsGoalReached())
            {
                double[] stateInput = ConvertStateToDoubleArray(State);
                double[] qValues = network.Compute(stateInput);
                int actionIndex = qValues.Select((v, i) => (v, i)).OrderByDescending(z => z.v).Select(x=> x.i).Except(usedActions).Last();
                //int actionIndex = Array.IndexOf(qValues, Enumerable.Max(qValues));
                var action = ValidActions[actionIndex];

                //Console.WriteLine(actionIndex);
                // Execute chosen action
                action.Execute(State);
                usedActions.Add(actionIndex);
                
            }
            Console.WriteLine("Goal reached");
            Console.WriteLine(State.Count(x => x.HasValue));
            var resultA = ChromosomeFromState(State, 20, _machines, _workers);
            return resultA;
            // while (!IsGoalReached())
            // {
            //     double[] stateInput = ConvertStateToDoubleArray(State);
            //     double[] qValues = network.Compute(stateInput);
            //     int actionIndex = Array.IndexOf(qValues, qValues.Max());
            //     var action = ValidActions[actionIndex];
            //     
            //     // Execute chosen action
            //     action.Execute(State);
            //     _currentChromosome = ChromosomeFromState(State, 20, _machines, _workers);
            // }
            // Console.WriteLine("Goal reached");
            // var result = new Chromosome(20, _machines.Length);
            // for (int i = 0; i < State.Length; i++)
            // {
            //     if (!State[i].HasValue)
            //         continue;
            //     var (day, worker) = PairingFunction(i);
            //     var machine = State[i].Value;
            //     result.Value[day][machine] = _workers.First(x => x.Id == worker);
            // }
            // return result;
        }
        
        private bool IsGoalReached()
        {
            var result = new Chromosome(20, _machines.Length);
            for (int i = 0; i < State.Length; i++)
            {
                if (!State[i].HasValue)
                    continue;
                var (day, worker) = PairingFunction(i);
                var machine = State[i].Value;
                result.Value[day][machine] = Enumerable.First(_workers, x => x.Id == worker);
            }
            return !result.Value.SelectMany(x => x).Any(x => x is null);
        }
        
        public void TrainAgent(int numberOfIterations)
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                InitializeEpisode();
                Console.WriteLine("Episode: " + i);
              
                //inputs.Clear();
                //outputs.Clear();
            }

            // for (int j = 0; j < numberOfIterations; j++)
            // {
            //     var error = teacher.RunEpoch(inputs.ToArray(), outputs.ToArray());
            //     Console.WriteLine("Error: " + error);
            //
            // }
        }
        
        private void InitializeEpisode()
        {
            InitializeChromosome();
            // for (int i = 0; i < 1000; i++)
            // {
            //     TakeAction();
            // }

            while (!IsGoalReached())
            {
                TakeAction();
            }

            Console.WriteLine($"{_currentChromosome.Fitness} {_currentChromosome.IsValid(_machines)}");
        }
        
        private void InitializeChromosome()
        {
              _currentChromosome = new Chromosome(20, _machines.Length);
             // for (int j = 0; j < 20; j++)
             // {
             //     for (int k = 0; k < _machines.Length; k++)
             //     {
             //         var qualifiedPeople = _workers.Where(x =>
             //             x.Qualifications != null &&
             //             x.Qualifications.Contains(_machines[k].RequiredQualification) &&
             //             !_currentChromosome.Value[j].Contains(x)).ToArray();
             //         _currentChromosome.Value[j][k] = qualifiedPeople[Random.Shared.Next(qualifiedPeople.Length)];
             //     }
             // }
             // _currentChromosome.RecalculateFitness();
             // Console.WriteLine("Initial fitness: " + _currentChromosome.Fitness);
            State = StateFromChromosome(_currentChromosome, 20, _machines, _workers);
        }
        private string i = "Hello";
        private List<double[]> inputs = new ();
        private List<double[]> outputs =  new();
        
        private void TakeAction()
        {
            // Get current Q values for the state.
            double[] stateInput = ConvertStateToDoubleArray(State);
            double[] currentQValues = network.Compute(stateInput);
            
            var action = GetRandomValidAction();
            if (action == null)
                return;
            
            // Get reward by simulating the action on a copy of the state.
            double reward = GetRewardBasedOnState(action);
            if (reward == 0) return;
            
            // Execute the action to update the state.
            action.Execute(State);
            
            // Get Q values for the new state.
            double[] newStateInput = ConvertStateToDoubleArray(State);
            double[] newQValues = network.Compute(newStateInput);
            double maxFutureQ = Enumerable.Max(newQValues);
            double targetQ = reward + (gamma * maxFutureQ);
            // Prepare the target vector: use the current Q-values for all actions,
            // but replace the chosen action's Q-value with the computed target.
            double[] targetVector = (double[])currentQValues.Clone();
            targetVector[action.Id] = targetQ;
            
            // Update the network with a single learning step.
            if (reward != 0)
            {
                //inputs.Add(stateInput);
                //outputs.Add(targetVector);
                //int actionIndex = currentQValues.Select((v, i) => (v, i)).OrderByDescending(z => z.v).First().i;

                //Console.WriteLine(currentQValues.Count(x=> x == maxFutureQ));
                //Console.WriteLine(actionIndex);
                var error = teacher.Run(newStateInput, targetVector);
                //Console.WriteLine("Error: " + error);
                 //var now = currentQValues.Select(x=>x.ToString()).Aggregate((c,z) => $"{c} {z}");
                 //Console.WriteLine($"Changed: {!now.Equals(i)}");
                 //i = now;
                 //Console.WriteLine($"Reward: {reward} TargetQ: {targetQ} MaxFutureQ: {maxFutureQ}");
            }
        }
        
        private QAction GetRandomValidAction()
        {
            var validActions = ValidActions
                .Where(a => State[PairingFunction(a.Day, a.Worker.Id)] is null)
                .ToList();

            //Console.WriteLine($" DUPA {validActions.Count()}");
            
            if (!validActions.Any())
                return null;
            
            int index = Random.Shared.Next(validActions.Count);
            return validActions[index];
        }
        
        public double GetRewardBasedOnState(QAction action)
        {
            var copyState = new int?[State.Length];
            State.CopyTo(copyState, 0);
            action.Execute(copyState);
            var current = _currentChromosome ?? ChromosomeFromState(State, 20, _machines, _workers);
            var newChromosome = ChromosomeFromState(copyState, 20, _machines, _workers);
            newChromosome.RecalculateFitness();
            if (_currentChromosome is null)
                current.RecalculateFitness();
            
            if(AreSame(current, newChromosome))
                return -100;
            
            double result = current.Fitness - newChromosome.Fitness;
            _currentChromosome = newChromosome;
            if (result == 0) result = 0.1;
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
        
        /// <summary>
        /// Converts the current state (an int? array) into a double array for network input.
        /// Null values are represented by -1.0.
        /// </summary>
        private double[] ConvertStateToDoubleArray(int?[] state)
        {
            double[] input = new double[state.Length];
            for (int i = 0; i < state.Length; i++)
            {
                input[i] = state[i].HasValue ? (double)state[i].Value : 0.0;
            }

            input = new double[_machines.Length * 20];
            for (int i = 0; i < state.Length; i++)
            {
                if (!state[i].HasValue)
                    continue;
                var (day, worker) = PairingFunction(i);
                var machine = state[i].Value;
                input[day * _machines.Length + machine] = worker;
            }
            
            return input;
        }
    }
}
