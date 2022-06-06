using System.Collections.Immutable;

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
                if (!Value[day][machineNumber].Qualifications.Contains(machines[machineNumber].RequiredQualification))
                    Fitness++;
                
            }
        }


        var workers = Value.SelectMany(x => x).Select(x => x).ToList();
        var dic = workers.Select(x => new {Person = x, Count = workers.Count(z => z.Id == x.Id)});
        var distinctBy = dic.DistinctBy(x=>x.Person.Id);
        foreach (var worker in distinctBy)
        {
            if (worker.Count is >= 14 or <= 10)
                Fitness += 1;
        }

        foreach (var day in Value)
        {
            var count = day.DistinctBy(x => x.Id).Count();
            if(count != day.Count())
                Fitness += 1;
        }

    }
}