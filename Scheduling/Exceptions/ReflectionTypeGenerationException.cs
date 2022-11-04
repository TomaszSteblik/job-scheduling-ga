using System;
using System.Runtime.Serialization;

namespace Scheduling.Exceptions;

[Serializable]
public class ReflectionTypeGenerationException : Exception
{
    public ReflectionTypeGenerationException() : base() { }

    public ReflectionTypeGenerationException(string message) :
        base($"{message}")
    { }

    protected ReflectionTypeGenerationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}