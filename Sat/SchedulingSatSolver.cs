using Google.OrTools.Sat;
using SchedulingAlgorithmModels.Models;

namespace Sat;

public class SchedulingSatSolver
{
    public Chromosome Run(Person[] people, Machine[] machines)
    {

        int numworkers = people.Length;
        int numShifts = machines.Length;
        int numDays = 20;

        Person[] allWorkers = people;
        int[] allDays = Enumerable.Range(0, numDays).ToArray();
        Machine[] allShifts = machines;
        

        // Creates the model.
        CpModel model = new CpModel();
        model.Model.Variables.Capacity = numworkers * numDays * numShifts;

        // Creates shift variables.
        // shifts[(n, d, s)]: worker 'n' works shift 's' on day 'd'.
        Dictionary<(int, int, int), BoolVar> shifts =
            new Dictionary<(int, int, int), BoolVar>(numworkers * numDays * numShifts);
        for (var i = 0; i < allWorkers.Length; i++)
        {
            var n = allWorkers[i];
            for (var j = 0; j < allDays.Length; j++)
            {
                var d = allDays[j];
                for (var k = 0; k < allShifts.Length; k++)
                {
                    var s = allShifts[k];
                    shifts.Add((i, d, k), model.NewBoolVar($"shifts_n{i}d{d}s{k}"));
                }
            }
        }

        // Each shift is assigned to exactly one worker in the schedule period.
        List<ILiteral> literals = new List<ILiteral>();
        for (var i = 0; i < allDays.Length; i++)
        {
            var d = allDays[i];
            for (var j = 0; j < allShifts.Length; j++)
            {
                var s = allShifts[j];
                for (var k = 0; k < allWorkers.Length; k++)
                {
                    var n = allWorkers[k];
                    literals.Add(shifts[(k, d, j)]);
                }

                model.AddExactlyOne(literals);
                literals.Clear();
            }
        }

        // Each worker works at most one shift per day.
        for (var i = 0; i < allWorkers.Length; i++)
        {
            var n = allWorkers[i];
            for (var j = 0; j < allDays.Length; j++)
            {
                var d = allDays[j];
                for (var k = 0; k < allShifts.Length; k++)
                {
                    var s = allShifts[k];
                    literals.Add(shifts[(i, d, k)]);
                }

                model.AddAtMostOne(literals);
                literals.Clear();
            }
        }

        for (var i = 0; i < allWorkers.Length; i++)
        {
            var n = allWorkers[i];
            for (var j = 0; j < allDays.Length; j++)
            {
                var d = allDays[j];
                for (var k = 0; k < allShifts.Length; k++)
                {
                    var s = allShifts[k];
                    if (s.RequiredQualification != null &&
                        !n.Qualifications.Any(q => q == s.RequiredQualification))
                    {
                        model.Add(shifts[(i, d, k)] == 0);
                    }
                }
            }
        }
        
        //Add constraint for preferred days count
        for (var i = 0; i < allWorkers.Length; i++)
        {
            var n = allWorkers[i];
            List<IntVar> preferredDaysWorked = new List<IntVar>();
            foreach (var d in n.PreferredDays)
            {
                for (var k = 0; k < allShifts.Length; k++)
                {
                    preferredDaysWorked.Add(shifts[(i, d, k)]);
                }
            }
            model.AddLinearConstraint(LinearExpr.Sum(preferredDaysWorked), 0, numDays);
        }

        // Define penalty variables
        Dictionary<(int, int, int), IntVar> dayPenalties = new Dictionary<(int, int, int), IntVar>();
        Dictionary<(int, int, int), IntVar> machinePenalties = new Dictionary<(int, int, int), IntVar>();

        for (var i = 0; i < allWorkers.Length; i++)
        {
            var n = allWorkers[i];
            for (var j = 0; j < allDays.Length; j++)
            {
                var d = allDays[j];
                for (var k = 0; k < allShifts.Length; k++)
                {
                    var s = allShifts[k];
                    // Penalty for not working on preferred days
                    if (!n.PreferredDays.Contains(d))
                    {
                        dayPenalties[(i, d, k)] = model.NewIntVar(0, 1, $"dayPenalty_n{i}d{d}s{k}");
                        model.Add(dayPenalties[(i, d, k)] == shifts[(i, d, k)]);
                    }

                    // Penalty for not working on preferred machines
                    if (!n.PreferredMachineIds.Contains(k))
                    {
                        machinePenalties[(i, d, k)] = model.NewIntVar(0, 1, $"machinePenalty_n{i}d{d}s{k}");
                        model.Add(machinePenalties[(i, d, k)] == shifts[(i, d, k)]);
                    }
                }
            }
        }

        // Objective: Minimize penalties
        IntVar totalDayPenalty = model.NewIntVar(0, numworkers * numDays * numShifts, "totalDayPenalty");
        IntVar totalMachinePenalty = model.NewIntVar(0, numworkers * numDays * numShifts, "totalMachinePenalty");

        model.Add(totalDayPenalty == LinearExpr.Sum(dayPenalties.Values));
        model.Add(totalMachinePenalty == LinearExpr.Sum(machinePenalties.Values));

        model.Minimize(totalDayPenalty + totalMachinePenalty);

        CpSolver solver = new CpSolver();
        // Tell the solver to enumerate all solutions.
        //solver.StringParameters += "linearization_level:0";

        // Display the first five solutions.
        const int solutionLimit = 20;
        SolutionPrinter printer = new SolutionPrinter(allWorkers, allDays, allShifts, shifts, solutionLimit);

        // Solve
        CpSolverStatus status = solver.Solve(model);
        // Console.WriteLine($"Solve status: {status}");
        //
        // Console.WriteLine("Statistics");
        // Console.WriteLine($"  conflicts: {solver.NumConflicts()}");
        // Console.WriteLine($"  branches : {solver.NumBranches()}");
        // Console.WriteLine($"  wall time: {solver.WallTime()}s");

        var result = new Chromosome(allDays.Length, allShifts.Length);
        
        for (var j = 0; j < allDays.Length; j++)
        {
            var d = allDays[j];
            //Console.WriteLine($"Day {d}");
            for (var k = 0; k < allShifts.Length; k++)
            {
                for (var i = 0; i < allWorkers.Length; i++)
                {
                    var n = allWorkers[i];
                    if (solver.BooleanValue(shifts[(n.Id, d, k)]))
                    {
                        result.Value[d][k] = n;
                        // Console.WriteLine(
                        //     $"  worker {n.Id} {n.Name} {n.Surname} works on machine {allShifts[k].Name}");
                    }
                }
            }
        }

        return result;

    }
}