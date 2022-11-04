using System;
using System.Runtime.Serialization;

namespace Scheduling.Exceptions;

[Serializable]
public class EmptyResultException : Exception
{
    public EmptyResultException() : base("Genetic algorithm returned null result.") { }

    public EmptyResultException(string message) :
        base($"Genetic algorithm returned null result. {message}")
    { }

    protected EmptyResultException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}