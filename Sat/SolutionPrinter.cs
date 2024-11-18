using Google.OrTools.Sat;
using SchedulingAlgorithmModels.Models;

namespace Sat;

public class SolutionPrinter : CpSolverSolutionCallback
{
    public SolutionPrinter(Person[] allNurses, int[] allDays, Machine[] allShifts,
        Dictionary<(int, int, int), BoolVar> shifts, int limit)
    {
        solutionCount_ = 0;
        allNurses_ = allNurses;
        allDays_ = allDays;
        allShifts_ = allShifts;
        shifts_ = shifts;
        solutionLimit_ = limit;
        Chromosome = new Chromosome(allDays.Length, allShifts.Length);
    }

    public override void OnSolutionCallback()
    {
        Console.WriteLine($"Solution #{solutionCount_}:");

        for (var j = 0; j < allDays_.Length; j++)
        {
            var d = allDays_[j];
            Console.WriteLine($"Day {d}");
            for (var k = 0; k < allShifts_.Length; k++)
            {
                for (var i = 0; i < allNurses_.Length; i++)
                {
                    var n = allNurses_[i];
                    if (Value(shifts_[(n.Id, d, k)]) == 1L)
                    {
                        Chromosome.Value[d][k] = n;
                        Console.WriteLine(
                            $"  Nurse {n.Id} {n.Name} {n.Surname} works on machine {allShifts_[k].Name}");
                    }
                }
            }
        }

        Chromosome.RecalculateFitness();
        Console.WriteLine($"Fitness: {Chromosome.Fitness}");
            
            
        solutionCount_++;
        if (solutionCount_ >= solutionLimit_)
        {
            Console.WriteLine($"Stop search after {solutionLimit_} solutions");
            StopSearch();
        }
    }

    public int SolutionCount()
    {
        return solutionCount_;
    }

    private int solutionCount_;
    private Person[] allNurses_;
    private int[] allDays_;
    private Machine[] allShifts_;
    private Dictionary<(int, int, int), BoolVar> shifts_;
    private int solutionLimit_;
    public Chromosome Chromosome { get; private set; }

}