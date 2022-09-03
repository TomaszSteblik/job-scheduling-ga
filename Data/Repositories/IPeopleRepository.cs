using Data.Dtos.Update;
using Data.Dtos.Read;
using Data.Dtos.Write;

namespace Data.Repositories;

public interface IPeopleRepository
{
    Task<bool> AddPerson(PersonWrite person);
    Task<PersonRead> GetPerson(int id);
    Task<ICollection<PersonRead>> GetPeople();
    Task<bool> UpdatePerson(PersonUpdate person);
}