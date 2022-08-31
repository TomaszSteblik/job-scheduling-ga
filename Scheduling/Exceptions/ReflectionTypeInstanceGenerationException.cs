using System;
using System.Runtime.Serialization;

namespace Scheduling.Exceptions;

[Serializable]
public class ReflectionTypeInstanceGenerationException : Exception
{
    public ReflectionTypeInstanceGenerationException() : base() { }
    
    public ReflectionTypeInstanceGenerationException(string message) : 
        base($"{message}") { }

    protected ReflectionTypeInstanceGenerationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}