using System.ComponentModel.DataAnnotations;

namespace BMSAPI.Models;

public class UserDTO {
    public string Id { get; set; }
}

public class CreateUserDTO {
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string UserName { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Lastname { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(200, MinimumLength = 2)]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [StringLength(200, MinimumLength = 2)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.PhoneNumber)]
    [StringLength(200, MinimumLength = 2)]
    public string PhoneNumber { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Country { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string City { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(4, MinimumLength = 4)]
    public string Zip { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Street { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string StreetNumber { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Floor { get; set; }
}

public class UpdateUserDTO : CreateUserDTO {
}