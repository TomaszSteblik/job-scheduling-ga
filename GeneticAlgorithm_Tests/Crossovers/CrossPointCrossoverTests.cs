using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Operators.Crossover;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace GeneticAlgorithm_Tests.Crossovers;

public class CrossPointCrossoverTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CrossPointCrossoverTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public void Test1()
    {
        var crossover = new CrossPointCrossover();
        var parents = new List<Chromosome>();
        var parentOne = new Chromosome(10, 4);
        for (var i = 0; i < parentOne.Value.Length; i++)
        {
            var days = parentOne.Value[i];
            for (var index = 0; index < days.Length; index++)
            {
                var person = new Person();
                person.Name = 2+i.ToString() + 2+index.ToString();
                days[index] = person;
            }
        }
        parents.Add(parentOne);
        var parentTwo = new Chromosome(10, 4);
        for (var i = 0; i < parentTwo.Value.Length; i++)
        {
            var days = parentTwo.Value[i];
            for (var index = 0; index < days.Length; index++)
            {
                var person = new Person();
                person.Name = 4+ (i).ToString() + 4+ (index).ToString();
                days[index] = person;
            }
        }
        parents.Add(parentTwo);
        var offsprings = crossover.GenerateOffsprings(parents);
        foreach (var offspring in offsprings)
        {
            _testOutputHelper.WriteLine("Offspring:");
            foreach (var days in offspring.Value)
            {
                string val = "";
                foreach (var person in days)
                {
                    val += " " + person + " ";
                }

                _testOutputHelper.WriteLine(val);
            }
        }
    }
}