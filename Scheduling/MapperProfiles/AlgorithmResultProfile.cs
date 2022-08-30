using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AutoMapper;
using GeneticAlgorithm.Models;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class AlgorithmResultProfile : Profile
{
    public AlgorithmResultProfile()
    {
        CreateMap<Result, AlgorithmResult>()
            .ForMember(x=>x.Schedule, 
                opt => opt.MapFrom(result => GenerateScheduleFromChromosome(result.Chromosome, result.Machines)));
    }

    private static IEnumerable<object> GenerateScheduleFromChromosome(Chromosome chromosome, Machine[] machines)
    {
        var propNames = machines.Select((x, i) => x.Name ?? $"Unknown_{i}").ToArray();
            
        var asmBuilder = GenerateTemporaryAssembly();
        var type = GenerateTemporaryType(propNames, asmBuilder);
        
        var scheduledDaysList = GenerateTypeList(chromosome.Value.Select(x => x.Select(z => z.Id.ToString()).ToArray()), 
            propNames, asmBuilder, type);
        
        return scheduledDaysList;
    }

    private static AssemblyBuilder GenerateTemporaryAssembly()
    {
        var assemblyName = new AssemblyName
        {
            Name = "DynamicObject"
        };
        var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        return asmBuilder;
    }

    private static Type GenerateTemporaryType(IEnumerable<string> names, AssemblyBuilder asmBuilder)
    {
        var modBuilder = asmBuilder.DefineDynamicModule("DynamicModule");
        var typeBuilder = modBuilder.DefineType("ScheduledDay", TypeAttributes.Public);

        foreach (var name in names)
        {
            var propertyBuilder = typeBuilder.DefineProperty(name,
                PropertyAttributes.None,
                CallingConventions.HasThis,
                typeof(string), Type.EmptyTypes);
            
            var backingFieldBuilder = typeBuilder.DefineField($"_machine.Name",
                typeof(string),
                FieldAttributes.Private);

            var getter = GenerateGetter(typeBuilder, name, backingFieldBuilder);
            propertyBuilder.SetGetMethod(getter);

            var setter = GenerateSetter(typeBuilder, name, backingFieldBuilder);
            propertyBuilder.SetSetMethod(setter);
        }

        var type = typeBuilder.CreateType() ?? throw new Exception("Creating temporary type failed during mapping");
        return type;
    }

    private static MethodBuilder GenerateSetter(TypeBuilder typeBuilder, string propName, FieldBuilder backingFieldBuilder)
    {
        var setterBuilder =
            typeBuilder.DefineMethod($"{propName}_getter",
                MethodAttributes.Public,
                null,
                new[] {typeof(string)});

        var setterIl = setterBuilder.GetILGenerator();

        setterIl.Emit(OpCodes.Ldarg_0);
        setterIl.Emit(OpCodes.Ldarg_1);
        setterIl.Emit(OpCodes.Stfld, backingFieldBuilder);
        setterIl.Emit(OpCodes.Ret);
        
        return setterBuilder;
    }

    private static MethodBuilder GenerateGetter(TypeBuilder typeBuilder, string propName, FieldBuilder backingFieldBuilder)
    {
        var getterBuilder = typeBuilder.DefineMethod($"{propName}_getter",
            MethodAttributes.Public, typeof(string), Type.EmptyTypes);

        var getterIl = getterBuilder.GetILGenerator();
        
        getterIl.Emit(OpCodes.Ldarg_0);
        getterIl.Emit(OpCodes.Ldfld, backingFieldBuilder);
        getterIl.Emit(OpCodes.Ret);
        
        return getterBuilder;
    }

    private static IEnumerable<object> GenerateTypeList(IEnumerable<string[]> values, string[] propNames, AssemblyBuilder asmBuilder, Type type)
    {
        var scheduledDaysList = new List<object>();
        foreach (var value in values)
        {
            var obj = asmBuilder.CreateInstance(type.FullName ??
                                                throw new Exception("Missing namespace during creation "))
                      ?? throw new Exception("Failed to create the temporary the object");
            for (var i = 0; i < propNames.Length; i++)
            {
                var name = propNames[i];
                var propInfo = type.GetProperty(name);
                propInfo?.SetValue(obj, value[i]);
            }

            scheduledDaysList.Add(obj);
        }

        return scheduledDaysList;
    }
}