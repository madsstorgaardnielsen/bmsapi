namespace BMSAPI.Database.Models;

public class Sleep : IEntity {
    public string Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool HeadPosition { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}