using Microsoft.AspNetCore.Identity;

namespace BMSAPI.Database.Models;

public class User : IdentityUser, IEntity {
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string Floor { get; set; }
    public List<Child> Children { get; set; }
}