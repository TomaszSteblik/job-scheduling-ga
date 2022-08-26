namespace GeneticAlgorithm.Exceptions;

public class PopulationNotInitializedException : ApplicationException
{
    public PopulationNotInitializedException(string populationFragment) : 
        base($"Population not initialized before accessing {populationFragment}")
    {
        
    }
}