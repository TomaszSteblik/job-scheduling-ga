using System;
using System.Collections.Generic;
using GeneticAlgorithm.Models;
using Xunit;

namespace GeneticAlgorithm_Tests.Models;

public class ChromosomeTests
{
    #region AnalyzePreferredDays
    
    [Fact]
    public void AnalyzePreferredDays_CorrectlyPlaced3Positions_Returns3()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person() {PreferredDays = new[] {0, 1}};
        var person2 = new Person() {PreferredDays = new[] {0, 2}};
        var person3 = new Person() {PreferredDays = new[] {1, 3}};
        var person4 = new Person() {PreferredDays = new[] {1, 0}};

        chromosome.Value = new[]
        {
            new[] {person1, person3},
            new[] {person4, person1},
            new[] {person4, person3},
            new[] {person2, person1},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredDays();
        
        //Assert
        Assert.Equal(3,actual);
    }
    
    [Fact]
    public void AnalyzePreferredDays_CorrectlyPlaced6Positions_Returns6()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person() {PreferredDays = new[] {0, 1}};
        var person2 = new Person() {PreferredDays = new[] {0, 2}};
        var person3 = new Person() {PreferredDays = new[] {1, 3}};
        var person4 = new Person() {PreferredDays = new[] {1, 3}};

        chromosome.Value = new[]
        {
            new[] {person1, person3},
            new[] {person4, person1},
            new[] {person2, person3},
            new[] {person3, person4},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredDays();
        
        //Assert
        Assert.Equal(6,actual);
    }
    
    [Fact]
    public void AnalyzePreferredDays_CorrectlyPlaced0Positions_Returns0()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person() {PreferredDays = new[] {2, 3}};
        var person2 = new Person() {PreferredDays = new[] {2, 3}};
        var person3 = new Person() {PreferredDays = new[] {1, 0}};
        var person4 = new Person() {PreferredDays = new[] {1, 0}};

        chromosome.Value = new[]
        {
            new[] {person1, person2},
            new[] {person2, person1},
            new[] {person4, person3},
            new[] {person3, person4},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredDays();
        
        //Assert
        Assert.Equal(0,actual);
    }
    
    [Fact]
    public void AnalyzePreferredDays_PeopleHaveNoPreferredDays_Returns0()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person();
        var person2 = new Person(){PreferredDays = new List<int>()};
        var person3 = new Person();
        var person4 = new Person();

        chromosome.Value = new[]
        {
            new[] {person1, person2},
            new[] {person2, person1},
            new[] {person4, person3},
            new[] {person3, person4},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredDays();
        
        //Assert
        Assert.Equal(0,actual);
    }
    
    #endregion

    #region AnalyzePreferredMachines

    [Fact]
    public void AnalyzePreferredMachines_PeopleHaveNoPreferredMachines_Returns0()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person();
        var person2 = new Person(){PreferredMachineIds = ArraySegment<int>.Empty};
        var person3 = new Person();
        var person4 = new Person();

        chromosome.Value = new[]
        {
            new[] {person1, person2},
            new[] {person2, person1},
            new[] {person4, person3},
            new[] {person3, person4},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredMachines();
        
        //Assert
        Assert.Equal(0,actual);
    }
    
    [Fact]
    public void AnalyzePreferredMachines_CorrectlyPlaced4Positions_Returns4()
    {
        //Arrange
        var chromosome = new Chromosome(4,2);
        var person1 = new Person(){PreferredMachineIds = new []{0}};
        var person2 = new Person(){PreferredMachineIds = new []{0,1}};
        var person3 = new Person(){PreferredMachineIds = new []{1}};
        var person4 = new Person(){PreferredMachineIds = new []{0}};

        chromosome.Value = new[]
        {
            new[] {person1, person2},
            new[] {person3, person4},
            new[] {person2, person1},
            new[] {person4, person1},
        };
        
        //Act
        var actual = chromosome.AnalyzePreferredMachines();
        
        //Assert
        Assert.Equal(4,actual);
    }

    #endregion
    
    #region AnalyzeMultipleMachines

    [Fact]
    public void AnalyzeMultipleMachines_EveryoneIsWorkingOnOneMachinePerDay_Returns0()
    {
        //Arrange
        var chromosome = new Chromosome(3,2);
        var person1 = new Person(){Id = 1};
        var person2 = new Person(){Id = 2};
        chromosome.Value = new[]
        {
            new[] {person1, person2},
            new[] {person1, person2},
            new[] {person1, person2}
        };
        
        //Act
        var actual = chromosome.AnalyzeMultipleMachines();
        
        //Assert
        Assert.Equal(0,actual);
    }
    
    [Fact]
    public void AnalyzeMultipleMachines_TwoPeopleAreWorkingOnTwoMachinesInOneDay_DifferentDays_Returns2()
    {
        //Arrange
        var chromosome = new Chromosome(3,2);
        var person1 = new Person(){Id = 1};
        var person2 = new Person(){Id = 2};
        chromosome.Value = new[]
        {
            new[] {person1, person1},
            new[] {person1, person2},
            new[] {person2, person2}
        };
        
        //Act
        var actual = chromosome.AnalyzeMultipleMachines();
        
        //Assert
        Assert.Equal(2,actual);
    }
    
    [Fact]
    public void AnalyzeMultipleMachines_TwoPeopleAreWorkingOnTwoMachinesInOneDay_SameDay_Returns1()
    {
        //Arrange
        var chromosome = new Chromosome(3,4);
        var person1 = new Person(){Id = 1};
        var person2 = new Person(){Id = 2};
        var person3 = new Person(){Id = 3};
        var person4 = new Person(){Id = 4};
        chromosome.Value = new[]
        {
            new[] {person1, person1,person2,person2},
            new[] {person1, person2,person3,person4},
            new[] {person1, person2,person3,person4},
        };
        
        //Act
        var actual = chromosome.AnalyzeMultipleMachines();
        
        //Assert
        Assert.Equal(1,actual);
    }
    
    #endregion

    #region AnalyzeWrongPosition

    [Fact]
    public void AnalyzeWrongPosition_EveryoneHaveCorrectQualifications_Returns0()
    {
        //Act
        var chromosome = new Chromosome(3,3);
        var machines = new[]
        {
            new Machine(){RequiredQualification = Qualification.Milling},
            new Machine(){RequiredQualification = Qualification.Sawing},
            new Machine(){RequiredQualification = Qualification.Painting}
        };
        var person1 = new Person() {Qualifications = new[] {Qualification.Milling, Qualification.Sawing}};
        var person2 = new Person() {Qualifications = new[] {Qualification.Sawing}};
        var person3 = new Person() {Qualifications = new[] {Qualification.Painting}};
        var person4 = new Person() {Qualifications = new[] {Qualification.Milling}};

        chromosome.Value = new[]
        {
            new []{person1,person2,person3},
            new []{person4,person1,person3},
            new []{person1,person2,person3},
        };

        //Act
        var actual = chromosome.AnalyzeWrongPosition(machines);

        //Assert
        Assert.Equal(0,actual);
    }
    
    [Fact]
    public void AnalyzeWrongPosition_OnePersonHaveWrongQualificationForMachineOnOneDay_Returns1()
    {
        //Act
        var chromosome = new Chromosome(3,3);
        var machines = new[]
        {
            new Machine(){RequiredQualification = Qualification.Milling},
            new Machine(){RequiredQualification = Qualification.Sawing},
            new Machine(){RequiredQualification = Qualification.Painting}
        };
        var person1 = new Person() {Qualifications = new[] {Qualification.Milling, Qualification.Sawing}};
        var person2 = new Person() {Qualifications = new[] {Qualification.Sawing}};
        var person3 = new Person() {Qualifications = new[] {Qualification.Painting}};
        var person4 = new Person() {Qualifications = new[] {Qualification.Milling}};

        chromosome.Value = new[]
        {
            new []{person1,person2,person3},
            new []{person4,person1,person2},
            new []{person1,person2,person3},
        };

        //Act
        var actual = chromosome.AnalyzeWrongPosition(machines);

        //Assert
        Assert.Equal(1,actual);
    }
    
    [Fact]
    public void AnalyzeWrongPosition_TwoPersonHaveWrongQualificationForMachineOnOneDay_DifferentDay_Returns2()
    {
        //Act
        var chromosome = new Chromosome(3,3);
        var machines = new[]
        {
            new Machine(){RequiredQualification = Qualification.Milling},
            new Machine(){RequiredQualification = Qualification.Sawing},
            new Machine(){RequiredQualification = Qualification.Painting}
        };
        var person1 = new Person() {Qualifications = new[] {Qualification.Milling, Qualification.Sawing}};
        var person2 = new Person() {Qualifications = new[] {Qualification.Sawing}};
        var person3 = new Person() {Qualifications = new[] {Qualification.Painting}};
        var person4 = new Person() {Qualifications = new[] {Qualification.Milling}};

        chromosome.Value = new[]
        {
            new []{person1,person4,person3},
            new []{person4,person1,person3},
            new []{person3,person2,person3},
        };

        //Act
        var actual = chromosome.AnalyzeWrongPosition(machines);

        //Assert
        Assert.Equal(2,actual);
    }

    [Fact]
    public void AnalyzeWrongPosition_ThreePersonHaveWrongQualificationForMachineOnOneDay_SameDay_Returns3()
    {
        //Act
        var chromosome = new Chromosome(3,3);
        var machines = new[]
        {
            new Machine(){RequiredQualification = Qualification.Milling},
            new Machine(){RequiredQualification = Qualification.Sawing},
            new Machine(){RequiredQualification = Qualification.Painting}
        };
        var person1 = new Person() {Qualifications = new[] {Qualification.Milling, Qualification.Sawing}};
        var person2 = new Person() {Qualifications = new[] {Qualification.Sawing}};
        var person3 = new Person() {Qualifications = new[] {Qualification.Painting}};
        var person4 = new Person() {Qualifications = new[] {Qualification.Milling}};

        chromosome.Value = new[]
        {
            new []{person1,person2,person3},
            new []{person3,person4,person1},
            new []{person1,person2,person3}
        };

        //Act
        var actual = chromosome.AnalyzeWrongPosition(machines);

        //Assert
        Assert.Equal(3,actual);
    }
    
    #endregion
}