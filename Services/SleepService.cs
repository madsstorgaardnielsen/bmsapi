using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services; 

public class SleepService {
    private readonly SleepRepository _sleepRepository;
    private readonly ChildRepository _childRepository;
    private readonly ILogger<SleepService> _logger;
    private readonly IMapper _mapper;

    public SleepService(SleepRepository sleepRepository,
        ILogger<SleepService> logger,
        IMapper mapper) {
        _sleepRepository = sleepRepository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<SleepDTO?> AddSleep(string username, CreateSleepDTO sleepDTO, CancellationToken ct) {
        var mappedSleep = _mapper.Map<Sleep>(sleepDTO);
        var sleep = await _sleepRepository.Create(mappedSleep, ct);

        if (sleep != null) {
            var child = await _childRepository.GetByChildId(username, sleepDTO.ChildId, ct);
            if (child != null) {
                sleep.Child = child;
                await _sleepRepository.SaveAsync(ct);
            }
        }

        return _mapper.Map<SleepDTO>(sleep);
    }

    public async Task<List<SleepDTO>> GetAllSleeps(string username, GetAllSleepsDTO sleepsDTO, CancellationToken ct) {
        var diapers = await _sleepRepository.GetAllSleeps(username, sleepsDTO, ct);
        return _mapper.Map<List<SleepDTO>>(diapers);
    }
    
    public async Task<bool> DeleteSleep(string sleepId, string username, CancellationToken ct) {
        return await _sleepRepository.Delete(sleepId, ct);
    }
}