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

public class CreateFeedingProfileDTO {
    public string Title { get; set; }
    public double TotalAmount { get; set; }
    public int TimesPrDay { get; set; }
}

public class SetFeedingProfileDTO {
    public string ProfileId { get; set; }
    public string ChildId { get; set; }
}