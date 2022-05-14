namespace BMSAPI.Database.Models;

public class Note : IEntity {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }
    public Child Child { get; set; }
    public string ChildId { get; set; }
    public byte[] Timestamp { get; set; }
}