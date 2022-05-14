
namespace BMSAPI.Models; 

public class SleepDTO {
    public string Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool HeadPosition { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}

public class CreateSleepDTO {
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool HeadPosition { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}