namespace BMSAPI.Database.Models;

public class Child : IEntity {
    public string Id { get; set; }
    public DateTime Birthdate { get; set; }
    public string Name { get; set; }
    public List<Measurement> Measurements { get; set; }
    public List<Feeding> Feedings { get; set; }
    public List<Diaper> Diapers { get; set; }
    public List<Sleep> Sleeps { get; set; }
    public List<Note> Notes { get; set; }
    // public List<FeedingProfile> FeedingProfiles { get; set; }
    public FeedingProfile FeedingProfile { get; set; }
    public List<User> Parents { get; set; }
    public byte[] Timestamp { get; set; }
}