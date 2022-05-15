namespace BMSAPI.Models; 

public class FeedingProfileDTO {
    public string Id { get; set; }
    public string Title { get; set; }
    public double TotalAmount { get; set; }
    public int TimesPrDay { get; set; }
    public string UserId { get; set; }
    public string ChildId { get; set; }
}

public class SimpleFeedingProfileDTO {
    public string Id { get; set; }
    public string Title { get; set; }
    public double TotalAmount { get; set; }
    public int TimesPrDay { get; set; }
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

public class CreateFeedingProfileDTO {
    public string Title { get; set; }
    public double TotalAmount { get; set; }
    public int TimesPrDay { get; set; }
}

public class SetFeedingProfileDTO {
    public string ProfileId { get; set; }
    public string ChildId { get; set; }
}