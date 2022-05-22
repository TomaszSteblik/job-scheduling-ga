namespace GeneticAlgorithm.Abstraction;

public interface IAlgorithm
{
    public Task RunAlgorithm();
    public Task ReloadSettings();
}