using System;
using System.Reactive;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dtos.Write;
using Data.Repositories;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Helpers;
using Scheduling.Models;
using Serilog;

namespace Scheduling.ViewModels;

public class AddQualificationViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IQualificationsRepository _qualificationsRepository;

    [Reactive]
    public Qualification Qualification { get; set; }

    public ReactiveCommand<Unit, Unit> AddCommand { get; set; }

    public AddQualificationViewModel(IMapper mapper, IQualificationsRepository qualificationsRepository)
    {
        _mapper = mapper;
        _qualificationsRepository = qualificationsRepository;
        Qualification = new Qualification();
        AddCommand = ReactiveCommand.CreateFromTask(AddQualification);
        AddCommand.LogExceptions();
    }

    private async Task AddQualification()
    {
        var qualificationWrite = _mapper.Map<QualificationWrite>(Qualification);
        await _qualificationsRepository.AddQualification(qualificationWrite);
    }
}