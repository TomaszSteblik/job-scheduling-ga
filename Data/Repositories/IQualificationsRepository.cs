using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;

namespace Data.Repositories;

public interface IQualificationsRepository
{
    Task<bool> AddQualification(QualificationWrite qualification);
    Task<QualificationRead> GetQualification(int id);
    Task<IEnumerable<QualificationRead>> GetQualifications();
    Task<bool> UpdateQualification(QualificationUpdate qualification);
}