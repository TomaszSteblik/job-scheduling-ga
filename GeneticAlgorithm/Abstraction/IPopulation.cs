using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Abstraction;

public interface IPopulation
{
    Chromosome[] GetAll();
    void RecalculateAll();
    void OrderByFitnessDesc();
    void Replace(int index, Chromosome chromosome);
    Machine[] GetMachines();
    Person[] GetPeople();
}