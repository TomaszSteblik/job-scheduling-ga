using SchedulingAlgorithmModels.Models;

namespace QLearning;

public class Utils
{
    public static int PairingFunction(int a, int b)
    {
        var result = ((a + b) * (a + b + 1)) / 2 + b;
        return result;
    }

    public static (int, int) PairingFunction(int z)
    {
        double wDouble = Math.Floor((Math.Sqrt(8 * z + 1) - 1) / 2);
        var w = (int)wDouble;
        
        // t is the triangular number for w: t = w*(w+1)/2
        var t = (w * (w + 1)) / 2;
        
        // b is the difference between z and the triangular number
        var b = z - t;
        
        // a is then recovered as: a = w - b
        var a = w - b;
        
        return (a, b);
    }
    
    public static Chromosome ChromosomeFromState(int?[] state, int days, Machine[] machines, Person[] workers)
    {
        var current = new Chromosome(days, machines.Length);
        for (var i = 0; i < state.Length; i++)
        {
            if(!state[i].HasValue)
                continue;
            var (day, worker) = Utils.PairingFunction(i);
            var machine = state[i].Value;

            current.Value[day][machine] = Enumerable.First(workers, x => x.Id == worker);

                
        }

        return current;
    }
    
    public static Chromosome ChromosomeFromState(float[] state, int days, Machine[] machines, Person[] workers)
    {
        var current = new Chromosome(days, machines.Length);

        for (int day = 0; day < current.Value.Length; day++)
        {
            for (int machine = 0; machine < current.Value[day].Length; machine++)
            {
                var index = (int) state[day * DeepQLearning.machines + machine];
                if(index < 0)
                    continue;
                current.Value[day][machine] = workers[index];
            }
        }
        
        return current;
    }
    
    
    public static int?[] StateFromChromosome(Chromosome chromosome, int days, Machine[] machines, Person[] workers)
    {
        int?[] state =new int?[Utils.PairingFunction(20, workers.Length)];
    
        for (int day = 0; day < days; day++)
        {
            for (int machine = 0; machine < machines.Length; machine++)
            {
                var worker = chromosome.Value[day][machine];
                if (worker != null)
                {
                    int index = Utils.PairingFunction(day, worker.Id);
                    state[index] = machine;
                }
            }
        }
    
        return state;
    }
}