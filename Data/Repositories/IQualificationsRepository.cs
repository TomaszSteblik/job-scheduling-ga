using Data.Models;

namespace Data.Repositories;

public interface IQualificationsRepository
{
    Task<bool> AddQualification(Qualification qualification);
    Task<Qualification> GetQualification(int id);
    Task<IEnumerable<Qualification>> GetQualifications();
    Task<bool> UpdateQualification(Qualification qualification);
}