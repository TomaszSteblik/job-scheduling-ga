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
                if (Value[day][machineNumber].Qualifications.Contains(machines[machineNumber].RequiredQualification))
                    Fitness--;
                else
                    Fitness++;
                //TODO:Add additional conditions for personel
            }
        }

    }
}