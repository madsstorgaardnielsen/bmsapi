using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Services;

public class AuthService : IAuthService {
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User _user;

    public AuthService(UserManager<User> userManager, IConfiguration configuration) {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> AuthenticateUser(LoginDTO loginDTO) {
        _user = await _userManager.FindByNameAsync(loginDTO.Username);
        return (_user != null && await _userManager.CheckPasswordAsync(_user, loginDTO.Password));
    }

    
}