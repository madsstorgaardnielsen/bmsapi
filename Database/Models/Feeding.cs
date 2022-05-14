namespace BMSAPI.Database.Models;

public class Feeding : IEntity {
    public string Id { get; set; }
    public DateTime DateTime { get; set; }
    public double Amount { get; set; }
    public bool Breast { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}