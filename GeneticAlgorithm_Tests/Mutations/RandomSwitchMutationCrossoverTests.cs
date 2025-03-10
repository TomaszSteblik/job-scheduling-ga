using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Infrastructure.Operators.Mutation;
using GeneticAlgorithm.Models;
using SchedulingAlgorithmModels.Models;
using Xunit;
using Xunit.Abstractions;

namespace GeneticAlgorithm_Tests.Mutations;

public class RandomSwitchMutationCrossoverTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private const string Milling = "Milling";
    private const string Sawing = "Sawing";
    private const string Painting = "Painting";

    public RandomSwitchMutationCrossoverTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Mutate_ChromosomeCorrectFitnessCorrected_ChromosomeValueChanged()
    {
        var population = GenerateMockPopulation(1, new Random(1));
        Assert.Equal(1, population.GetAll().First().Value[18][1].Id);
        Assert.Equal(2, population.GetAll().First().Value[1][0].Id);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        population.RecalculateAll();
        var mutation = new RandomSwitchMutation(population, new Random(9301));
        mutation.MutatePopulation(1);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        Assert.Equal(2, population.GetAll().First().Value[18][1].Id);
        Assert.Equal(1, population.GetAll().First().Value[1][0].Id);
    }

    [Fact]
    public void Mutate_ChromosomeCorrectFitnessWorsen_ChromosomeValueNotChanged()
    {
        var population = GenerateMockPopulation(1, new Random(1));
        Assert.Equal(3, population.GetAll().First().Value[13][0].Id);
        Assert.Equal(2, population.GetAll().First().Value[17][1].Id);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        population.RecalculateAll();
        var mutation = new RandomSwitchMutation(population, new Random(3));
        mutation.MutatePopulation(1);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        Assert.Equal(3, population.GetAll().First().Value[13][0].Id);
        Assert.Equal(2, population.GetAll().First().Value[17][1].Id);
    }

    [Fact]
    public void Mutate_ChromosomeIncorrect_ChromosomeValueNotChanged()
    {
        var population = GenerateMockPopulation(1, new Random(1));
        Assert.Equal(1, population.GetAll().First().Value[2][1].Id);
        Assert.Equal(2, population.GetAll().First().Value[9][1].Id);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        population.RecalculateAll();
        var mutation = new RandomSwitchMutation(population, new Random(1));
        mutation.MutatePopulation(1);
        Assert.True(population.GetAll().First().IsValid(population.GetMachines()));
        Assert.Equal(1, population.GetAll().First().Value[2][1].Id);
        Assert.Equal(2, population.GetAll().First().Value[9][1].Id);
    }

    private static Population GenerateMockPopulation(int size, Random random)
    {
        var mockPopulation = new Population(random);
        mockPopulation.InitializePopulation(
            new[]{
                new Machine {Name = "Machine1",RequiredQualification = Milling},
                new Machine {Name = "Machine2",RequiredQualification = Milling}
            },
            new[]
            {
                new Person(){Id = 1,Name = "Person1", Surname = "Kowalski1", PreferenceDaysCount = 20,
                    PreferredMachineIds = new[] {0},Qualifications = new List<string>() {Milling}},
                new Person(){Id = 2,Name = "Person2", Surname = "Kowalski2", PreferenceDaysCount = 20,
                    PreferredMachineIds = new[] {1},Qualifications = new List<string>() {Milling}},
                new Person(){Id = 3,Name = "Person3", Surname = "Kowalski3", PreferenceDaysCount = 20,
                    PreferredMachineIds = new[] {0},Qualifications = new List<string>() {Milling}},
            },
            size);
        return mockPopulation;
    }

}