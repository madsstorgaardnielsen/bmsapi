namespace BMSAPI.Database.Models;

public class Measurement : IEntity {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public double HeadCircumference { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
}