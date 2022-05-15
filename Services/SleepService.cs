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

    public SleepService(SleepRepository sleepRepository, ChildRepository childRepository,
        ILogger<SleepService> logger,
        IMapper mapper) {
        _sleepRepository = sleepRepository;
        _childRepository = childRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AverageTimeSleptDTO> GetDailyAverageTimeSlept(string username, GetAllSleepsDTO sleepsDTO,
        CancellationToken ct) {
        var sleeps = await _sleepRepository.GetAllSleeps(username, sleepsDTO, ct);
        
        var timeSlept = new List<TimeSpan>();
        var totalTimeSlept = new TimeSpan();

        foreach (var sleep in sleeps) {
            Console.WriteLine("test");
            var timespan = sleep.To.Subtract(sleep.From);
            timeSlept.Add(timespan);
            totalTimeSlept = totalTimeSlept.Add(timespan);
        }

        var doubleAverageTicks = timeSlept.Average(t => t.Ticks);
        var longAverageTicks = Convert.ToInt64(doubleAverageTicks);

        var res = new TimeSpan(longAverageTicks);

        return new AverageTimeSleptDTO {
            DailyAverageSleep = res, TotalSleepInPeriod = totalTimeSlept, FromDate = sleepsDTO.From,
            ToDate = sleepsDTO.To
        };
    }

    public async Task<DailySleepStatusDTO> GetDailyStatus(string username, string childId, CancellationToken ct) {
        var sleeps = await _sleepRepository.GetAllSleeps(username,
            new GetAllSleepsDTO
                {From = DateTime.Now.Date, To = DateTime.Now.Date.AddHours(23).AddMinutes(55), ChildId = childId}, ct);

        var totalSleep = new TimeSpan();

        foreach (var sleep in sleeps) {
            var timespan = sleep.To.Subtract(sleep.From);
            totalSleep = totalSleep.Add(timespan);
        }

        return new DailySleepStatusDTO {CurrentTimeSlept = totalSleep};
    }

    public async Task<SleepDTO?> AddSleep(string username, CreateSleepDTO sleepDTO, CancellationToken ct) {
        var mappedSleep = _mapper.Map<Sleep>(sleepDTO);
        var sleep = await _sleepRepository.Create(mappedSleep, ct);

        if (sleep != null) {
            var child = await _childRepository.GetByChildId(username, sleepDTO.ChildId, ct);
            if (child != null) {
                child.Sleeps.Add(sleep);
                await _childRepository.SaveAsync(ct);
            }
        }

        return _mapper.Map<SleepDTO>(sleep);
    }

    public async Task<List<SimpleSleepDTO>> GetAllSleeps(string username, GetAllSleepsDTO sleepsDTO,
        CancellationToken ct) {
        var sleeps = await _sleepRepository.GetAllSleeps(username, sleepsDTO, ct);
        return _mapper.Map<List<SimpleSleepDTO>>(sleeps);
    }

    public async Task<bool> DeleteSleep(string sleepId, string username, CancellationToken ct) {
        return await _sleepRepository.Delete(sleepId, ct);
    }
}