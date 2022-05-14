namespace BMSAPI.Database.Models;

public interface IEntity {
    public string Id { get; set; }
    public byte[] Timestamp { get; set; }
}