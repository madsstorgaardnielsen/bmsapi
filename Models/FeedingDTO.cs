using BMSAPI.Database.Models;

namespace BMSAPI.Models; 

public class FeedingDTO {
    public string Id { get; set; }
    public DateTime DateTime { get; set; }

    public double Amount { get; set; }
    public bool Breast { get; set; }
}

public class CreateFeedingDTO {
    public DateTime DateTime { get; set; }
    public double Amount { get; set; }
    public bool Breast { get; set; }
    public string ChildId { get; set; }
}

public class GetAllFeedingDTO {
    public string? ChildId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}