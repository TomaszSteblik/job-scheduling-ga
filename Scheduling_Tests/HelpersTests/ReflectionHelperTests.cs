using System.Reflection;
using System.Reflection.Emit;
using Scheduling.Exceptions;
using Scheduling.Helpers;

namespace Scheduling_Tests.HelpersTests;

public class ReflectionHelperTests
{
    [Fact]
    public void GenerateTypeList_ExistingTypeWithSymmetricValuesAndParams_ReturnsIEnumerable()
    {
        //Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var values = new List<string[]>()
        {
            new[]{"A","B"},
            new[]{"C","D"},
            new[]{"E","F"},
        };
        var propNames = new[] { "Test1", "Test2" };
        //Act 
        var list = ReflectionHelper.GenerateTypeList(values, propNames, assembly, typeof(TestType)).ToArray();
        //Assert
        Assert.NotEmpty(list);
        Assert.IsType<TestType>(list.First());
        Assert.Equal(3, list.Length);
        Assert.Equal("A", (list[0] as TestType)?.Test1);
        Assert.Equal("B", (list[0] as TestType)?.Test2);
        Assert.Equal("C", (list[1] as TestType)?.Test1);
        Assert.Equal("D", (list[1] as TestType)?.Test2);
        Assert.Equal("E", (list[2] as TestType)?.Test1);
        Assert.Equal("F", (list[2] as TestType)?.Test2);


    }

    [Fact]
    public void GenerateTypeList_NonExistingType_ThrowsException()
    {
        //Arrange
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() { Name = "E" }, AssemblyBuilderAccess.Run);
        var values = new List<string[]>()
        {
            new[]{"A","B"},
            new[]{"C","D"},
            new[]{"E","F"},
        };
        var propNames = new[] { "Test1", "Test2" };
        //Act & Assert
        Assert.Throws<ReflectionTypeInstanceGenerationException>(() => ReflectionHelper.GenerateTypeList(values, propNames, assembly, typeof(TestType)));
    }

    [Fact]
    public void GenerateTypeList_ExistingTypeWithAsymmetricValuesAndParams_ThrowsException()
    {
        //Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var values = new List<string[]>()
        {
            new[]{"A"},
            new[]{"C"},
            new[]{"E"},
        };
        var propNames = new[] { "Test1", "Test2" };
        //Act 
        var exception = Assert.Throws<ReflectionTypeInstanceGenerationException>(() => ReflectionHelper.GenerateTypeList(values, propNames, assembly, typeof(TestType)));
        //Assert
        Assert.Equal("Asymmetric values and prop names. Every prop name need matching value", exception.Message);
    }

    [Fact]
    public void GenerateTemporaryType_WithEmptyPropNames_ReturnsTypeWithoutProps()
    {
        //Arrange
        const string name = "TestType";
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() { Name = "E" }, AssemblyBuilderAccess.Run);
        //Act
        var type = ReflectionHelper.GenerateTemporaryType(new List<string>(), assembly, name);
        //Assert
        Assert.Empty(type.GetProperties());
    }

    [Fact]
    public void GenerateTemporaryType_WithPropNames_ReturnsTypeWithProps()
    {
        //Arrange
        const string name = "TestType";
        var propNames = new[] { "A", "B" };
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() { Name = "E" }, AssemblyBuilderAccess.Run);
        //Act
        var type = ReflectionHelper.GenerateTemporaryType(propNames, assembly, name);
        var propertyInfoA = type.GetProperty("A");
        var propertyInfoB = type.GetProperty("B");
        //Assert
        Assert.NotEmpty(type.GetProperties());
        Assert.Equal(2, type.GetProperties().Length);
        Assert.Equal("A", type.GetProperties()[0].Name);
        Assert.Equal("B", type.GetProperties()[1].Name);
        Assert.NotNull(propertyInfoA);
        Assert.NotNull(propertyInfoB);
        Assert.True(propertyInfoA?.CanWrite);
        Assert.True(propertyInfoB?.CanWrite);
        Assert.True(propertyInfoA?.CanRead);
        Assert.True(propertyInfoB?.CanRead);
    }

    [Fact]
    public void GenerateTemporaryType_WithNullName_ThrowsException()
    {
        //Arrange
        string name = null!;
        var propNames = new[] { "A", "B" };
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() { Name = "E" }, AssemblyBuilderAccess.Run);
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => ReflectionHelper.GenerateTemporaryType(propNames, assembly, name));
    }

    [Fact]
    public void GenerateTemporaryAssembly_WithName_ReturnsAssemblyBuilderWithName()
    {
        //Arrange
        const string name = "TestAssemblyName";
        //Act
        var assembly = ReflectionHelper.GenerateTemporaryAssembly(name);
        var actualName = assembly.GetName();
        //Assert
        Assert.NotNull(assembly);
        Assert.NotNull(actualName);
        Assert.NotNull(actualName.Name);
        Assert.Equal(name, actualName.Name);
    }

    [Fact]
    public void GenerateTemporaryAssembly_WithNullName_ThrowsException()
    {
        //Arrange
        string name = null!;
        //Act & Assert
        Assert.Throws<ArgumentException>(() => ReflectionHelper.GenerateTemporaryAssembly(name));
    }
}

public class TestType
{
    public string? Test1 { get; set; }
    public string? Test2 { get; set; }
}