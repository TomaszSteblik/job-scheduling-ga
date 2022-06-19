using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeneticAlgorithm.Infrastructure.Operators.Crossover;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Operators.Crossover;
using Moq;
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
    public void CrossPointDayCrossover_Test()
    {
        //Arrange
        var crossover = new CrossPointDayCrossover(new Random(1));
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
        foreach (var offspring in parents)
        {
            _testOutputHelper.WriteLine("Parent:");
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
        
        //Act
        var offsprings = crossover.GenerateOffsprings(parents);
        offsprings = crossover.GenerateOffsprings(offsprings);
        offsprings = crossover.GenerateOffsprings(offsprings);
        offsprings = crossover.GenerateOffsprings(offsprings);
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
        
        //Assert
        Assert.Equal(offsprings[0].Value[0],parentOne.Value[0]);
        Assert.Equal(offsprings[1].Value[0],parentTwo.Value[0]);
        
        Assert.Equal(offsprings[0].Value[1],parentTwo.Value[1]);
        Assert.Equal(offsprings[1].Value[1],parentOne.Value[1]);
        
        Assert.Equal(offsprings[0].Value[2],parentTwo.Value[2]);
        Assert.Equal(offsprings[1].Value[2],parentOne.Value[2]);
        
        Assert.Equal(offsprings[0].Value[3],parentOne.Value[3]);
        Assert.Equal(offsprings[1].Value[3],parentTwo.Value[3]);
        
        Assert.Equal(offsprings[0].Value[4],parentOne.Value[4]);
        Assert.Equal(offsprings[1].Value[4],parentTwo.Value[4]);
        
        Assert.Equal(offsprings[0].Value[5],parentTwo.Value[5]);
        Assert.Equal(offsprings[1].Value[5],parentOne.Value[5]);
        
        Assert.Equal(offsprings[0].Value[6],parentTwo.Value[6]);
        Assert.Equal(offsprings[1].Value[6],parentOne.Value[6]);
        
        Assert.Equal(offsprings[0].Value[7],parentOne.Value[7]);
        Assert.Equal(offsprings[1].Value[7],parentTwo.Value[7]);
        
        Assert.Equal(offsprings[0].Value[8],parentOne.Value[8]);
        Assert.Equal(offsprings[1].Value[8],parentTwo.Value[8]);
        
        Assert.Equal(offsprings[0].Value[9],parentOne.Value[9]);
        Assert.Equal(offsprings[1].Value[9],parentTwo.Value[9]);

    }
    
     [Fact]
    public void CrossPointMachineCrossover_Test()
    {
        //Arrange
        var crossover = new CrossPointMachineCrossover(new Random(1));
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
        foreach (var offspring in parents)
        {
            _testOutputHelper.WriteLine("Parent:");
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
        
        //Act
        var offsprings = crossover.GenerateOffsprings(parents);
        offsprings = crossover.GenerateOffsprings(offsprings);
        offsprings = crossover.GenerateOffsprings(offsprings);
        offsprings = crossover.GenerateOffsprings(offsprings);
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
        
        //Assert
        Assert.Equal(parentOne.Value.Select(x=>x[0]),offsprings[0].Value.Select(x=>x[0]));
        Assert.Equal(parentTwo.Value.Select(x=>x[0]),offsprings[1].Value.Select(x=>x[0]));
        
        Assert.Equal(parentOne.Value.Select(x=>x[1]),offsprings[0].Value.Select(x=>x[1]));
        Assert.Equal(parentTwo.Value.Select(x=>x[1]),offsprings[1].Value.Select(x=>x[1]));
        
        Assert.Equal(parentTwo.Value.Select(x=>x[2]),offsprings[0].Value.Select(x=>x[2]));
        Assert.Equal(parentOne.Value.Select(x=>x[2]),offsprings[1].Value.Select(x=>x[2]));
        
        Assert.Equal(parentOne.Value.Select(x=>x[3]),offsprings[0].Value.Select(x=>x[3]));
        Assert.Equal(parentTwo.Value.Select(x=>x[3]),offsprings[1].Value.Select(x=>x[3]));
        
    }
}