using System.Runtime.Serialization;

namespace GeneticAlgorithm.Exceptions;

[Serializable]
public class PopulationNotInitializedException : Exception
{
    public PopulationNotInitializedException() : base()
    {
        
    }
    
    public PopulationNotInitializedException(string populationFragment) : 
        base($"Population not initialized before accessing {populationFragment}")
    {
        
    }

    protected PopulationNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}