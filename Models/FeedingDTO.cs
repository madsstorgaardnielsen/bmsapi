using System.ComponentModel.DataAnnotations;
using BMSAPI.Database.Models;

namespace BMSAPI.Models;

public class FeedingDTO {
    public string Id { get; set; }
    public DateTime DateTime { get; set; }

    public double Amount { get; set; }
    public bool Breast { get; set; }
}

public class UpdateFeedingDTO {
    [Required] public string Id { get; set; }
    [Required] public DateTime DateTime { get; set; }
    [Required] public double Amount { get; set; }
    [Required] public bool Breast { get; set; }
    [Required] public string ChildId { get; set; }
}

public class AverageIntakeDTO {
    public double DailyAverageIntake { get; set; }
    public double TotalIntakeInPeriod { get; set; }
    public int AmountTimesLeftBreast { get; set; }
    public int AmountTimesRightBreast { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class DailyIntakeStatusDTO {
    public double CurrentAmount { get; set; }
    public double NeededAmount { get; set; }
    public double Difference { get; set; }
}

public class CreateFeedingDTO {
    [Required] public DateTime DateTime { get; set; }
    [Required] public double Amount { get; set; }
    [Required] public bool Breast { get; set; }
    [Required] public string ChildId { get; set; }
}

public class GetAllFeedingDTO {
    public string? ChildId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}