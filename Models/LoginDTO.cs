using System.ComponentModel.DataAnnotations;

namespace BMSAPI.Models;

public class LoginDTO {
    [Required]
    [DataType(DataType.Text)]
    [StringLength(200, MinimumLength = 2)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(200, MinimumLength = 2)]
    public string Password { get; set; }
}

public class ChangePasswordDTO : LoginDTO {
    [Required]
    [DataType(DataType.Password)]
    [StringLength(200, MinimumLength = 2)]
    public string NewPassword { get; set; }
}