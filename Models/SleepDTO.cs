using System.ComponentModel.DataAnnotations;

namespace BMSAPI.Models;

public class SleepDTO {
    public string Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool HeadPosition { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}

public class SimpleSleepDTO {
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool HeadPosition { get; set; }
}

public class DailySleepStatusDTO {
    public TimeSpan CurrentTimeSlept { get; set; }
}

public class AverageTimeSleptDTO {
    public TimeSpan DailyAverageSleep { get; set; }
    public TimeSpan TotalSleepInPeriod { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CreateSleepDTO {
    [Required] public DateTime From { get; set; }
    [Required] public DateTime To { get; set; }
    [Required] public bool HeadPosition { get; set; }
    [Required] public string ChildId { get; set; }
}

public class GetAllSleepsDTO {
    public string? ChildId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}