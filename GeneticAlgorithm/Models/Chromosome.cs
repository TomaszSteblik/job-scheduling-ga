using System.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GeneticAlgorithm_Tests")]
namespace GeneticAlgorithm.Models;

public class Chromosome
{
    public Person[][] Value { get; set; }
    public double Fitness { get; set; }
    private readonly int _positionsCount;

    public Chromosome(int daysCount, int machinesCount)
    {
        Value = new Person[daysCount][];
        for (var i = 0; i < daysCount; i++)
        {
            Value[i] = new Person[machinesCount];
        }

        _positionsCount = daysCount * machinesCount;
    }

    internal void RecalculateFitness(Machine[]? machines)
    {
        Fitness = 0;
        RecalculateFitnessByDaysWorking();
        RecalculateFitnessByPreferredMachine();
        RecalculateFitnessByPreferredDays();
    }

    private void RecalculateFitnessByPreferredDays()
    {
        Fitness += _positionsCount;
        Fitness -= AnalyzePreferredDays();
    }

    ///<summary>
    ///Method returns sum of correctly placed people in all days according to preferred days
    ///</summary>
    internal int AnalyzePreferredDays()
    {
        var result = 0;
        for (var i = 0; i < Value.Length; i++)
        {
            for (var j = 0; j < Value[i].Length; j++)
            {
                var preferredDays = Value[i][j].PreferredDays;
                if (preferredDays is null) continue;

                if (preferredDays.Contains(i))
                    result++;
            }
        }
        return result;
    }

    private void RecalculateFitnessByPreferredMachine()
    {
        Fitness += _positionsCount;
        Fitness -= AnalyzePreferredMachines();
    }

    ///<summary>
    ///Method returns sum of correctly placed people in all days according to preferred machines
    ///</summary>
    internal int AnalyzePreferredMachines()
    {
        var result = 0;
        for (var i = 0; i < Value.Length; i++)
        {
            for (var j = 0; j < Value[i].Length; j++)
            {
                var preferredMachineIds = Value[i][j].PreferredMachineIds;
                if (preferredMachineIds is null) continue;
                
                if (preferredMachineIds.Contains(j))
                    result++;
            }
        }
        return result;
    }

    private void RecalculateFitnessByDaysWorking()
    {
        var workers = Value.SelectMany(x => x).Select(x => x).ToList();
        var dic = workers.Select(x => new {Person = x, Count = workers.Count(z => z.Id == x.Id)});
        var distinctBy = dic.DistinctBy(x => x.Person.Id);
        foreach (var worker in distinctBy)
        {
            var daysDifference = Math.Abs(worker.Count - worker.Person.PreferenceDaysCount);
            Fitness += daysDifference;
        }
    }

    ///<summary>
    ///Method returns sum of days where there is at least one person working on two or more machines in the same day
    ///</summary>
    internal int AnalyzeMultipleMachines()
    {
        var fitness = 0;
        foreach (var day in Value)
        {
            var count = day.DistinctBy(x => x.Id).Count();
            if(count != day.Length)
                fitness += 1;
        }

        return fitness;
    }
    
    internal int AnalyzeWrongPosition(Machine[]? machines)
    {
        var fitness = 0;
        for (var day = 0; day < Value.Length; day++)
        {
            for (var machineNumber = 0; machineNumber < machines.Length; machineNumber++)
            {
                var qualifications = Value[day][machineNumber].Qualifications;
                if (qualifications is null)
                    throw new DataException(
                        $"Worker doesn't have any qualifications. day: {day}, machine: {machineNumber}");
                if (!qualifications.Contains(machines[machineNumber].RequiredQualification))
                    fitness++;
            }
        }

        return fitness;
    }

    public bool IsValid(Machine[]? machines)
    {
        return AnalyzeWrongPosition(machines) == 0 && AnalyzeMultipleMachines() == 0;
    }
}