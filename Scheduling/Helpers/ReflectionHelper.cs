using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Scheduling.Exceptions;

namespace Scheduling.Helpers;

public static class ReflectionHelper
{
    public static AssemblyBuilder GenerateTemporaryAssembly(string assemblyName)
    {
        var name = new AssemblyName
        {
            Name = assemblyName
        };
        var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
        return asmBuilder;
    }

    public static Type GenerateTemporaryType(IEnumerable<string> names, AssemblyBuilder asmBuilder, string typeName)
    {
        var modBuilder = asmBuilder.DefineDynamicModule($"{typeName}Module");
        var typeBuilder = modBuilder.DefineType(typeName, TypeAttributes.Public);

        foreach (var name in names)
        {
            var propertyBuilder = typeBuilder.DefineProperty(name,
                PropertyAttributes.None,
                CallingConventions.HasThis,
                typeof(string), Type.EmptyTypes);
            
            var backingFieldBuilder = typeBuilder.DefineField($"_{name}",
                typeof(string),
                FieldAttributes.Private);

            var getter = GenerateGetter(typeBuilder, name, backingFieldBuilder);
            propertyBuilder.SetGetMethod(getter);

            var setter = GenerateSetter(typeBuilder, name, backingFieldBuilder);
            propertyBuilder.SetSetMethod(setter);
        }

        var type = typeBuilder.CreateType() ?? throw new ReflectionTypeGenerationException("Creating temporary type failed during mapping");
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

    public static IEnumerable<object> GenerateTypeList(IEnumerable<string[]> values, string[] propNames, Assembly asmBuilder, Type type)
    {
        var scheduledDaysList = new List<object>();
        foreach (var value in values)
        {
            if (value.Length != propNames.Length)
                throw new ReflectionTypeInstanceGenerationException("Asymmetric values and prop names. Every prop name need matching value");
            var obj = asmBuilder.CreateInstance(type.FullName ??
                                                throw new ReflectionTypeInstanceGenerationException("Missing namespace during creation "))
                      ?? throw new ReflectionTypeInstanceGenerationException("Failed to create the temporary the object");
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