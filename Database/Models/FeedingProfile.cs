namespace BMSAPI.Database.Models;

public class FeedingProfile : IEntity {
    public string Id { get; set; }
    public string Title { get; set; }
    public double TotalAmount { get; set; }
    public int TimesPrDay { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
}