using System.Collections;
using Data.Models;

namespace Data.Repositories;

public interface IPeopleRepository
{
    Task<bool> AddPerson(Person person);
    Task<Person> GetPerson(int id);
    Task<ICollection<Person>> GetPeople();
    Task<bool> UpdatePerson(Person person);
}