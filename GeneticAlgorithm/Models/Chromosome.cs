using System.Data;

namespace GeneticAlgorithm.Models;

public class Chromosome
{
    public Person[][] Value { get; set; }
    public double Fitness { get; set; }

    public Chromosome(int daysCount, int machinesCount)
    {
        Value = new Person[daysCount][];
        for (int i = 0; i < daysCount; i++)
        {
            Value[i] = new Person[machinesCount];
        }
    }

    public void RecalculateFitness(Machine[] machines)
    {
        Fitness = 0;
        for (var day = 0; day < Value.Length; day++)
        {
            for (var machineNumber = 0; machineNumber < machines.Length; machineNumber++)
            {
                var qualifications = Value[day][machineNumber].Qualifications;
                if (qualifications is null)
                    throw new DataException(
                        $"Worker doesn't have any qualifications. day: {day}, machine: {machineNumber}");
                if (!qualifications.Contains(machines[machineNumber].RequiredQualification))
                    Fitness++;
                
            }
        }


        var workers = Value.SelectMany(x => x).Select(x => x).ToList();
        var dic = workers.Select(x => new {Person = x, Count = workers.Count(z => z.Id == x.Id)});
        var distinctBy = dic.DistinctBy(x=>x.Person.Id);
        foreach (var worker in distinctBy)
        {
            if (worker.Count is >= 14 or <= 10)
                Fitness += 10;
        }

        foreach (var day in Value)
        {
            var count = day.DistinctBy(x => x.Id).Count();
            if(count != day.Length)
                Fitness += 100;
        }

    }
    
    public int AnalyzeMultipleMachines()
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
    
    public int AnalyzeWrongPosition(Machine[] machines)
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
}