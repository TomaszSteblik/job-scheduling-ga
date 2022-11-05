using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Scheduling.Models;

namespace Scheduling.Repositories;

public class DataRepository : IDataRepository
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IMachinesRepository _machinesRepository;
    private readonly IMapper _mapper;

    public DataRepository(IPeopleRepository peopleRepository, IMachinesRepository machinesRepository, IMapper mapper)
    {
        _peopleRepository = peopleRepository;
        _machinesRepository = machinesRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<Worker>> GetWorkers()
    {
        var people = await _peopleRepository.GetPeople();
        return _mapper.Map<IEnumerable<Worker>>(people);
    }

    public async Task<IEnumerable<Machine>> GetMachines()
    {
        var machines = await _machinesRepository.GetMachines();
        return _mapper.Map<IEnumerable<Machine>>(machines);
    }
}