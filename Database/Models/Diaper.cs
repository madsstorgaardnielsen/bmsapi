namespace BMSAPI.Database.Models;

public class Diaper : IEntity {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public bool Wet { get; set; }
    public bool Poop { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
}