using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BMSAPI.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Services;

public class JWTService {
    private readonly IConfiguration _configuration;
    private UserManager<User> _userManager;

    public JWTService(UserManager<User> userManager, IConfiguration configuration) {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> CreateToken(string username) {
        var user = await _userManager.FindByNameAsync(username);
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var token = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims) {
        var jwtSettings = _configuration.GetSection("JwtToken");

        var expiration = DateTime
            .Now
            .AddDays(14); //TODO 

        var issuer = jwtSettings.GetSection("ValidIssuer").Value;

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        return token;
    }

    private async Task<List<Claim>> GetClaims(User user) {
        var claims = new List<Claim> {
            new("Id", user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Expiration, DateTime.Now.AddDays(14).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles
            .Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private SigningCredentials GetSigningCredentials() {
        var jwtSettings = _configuration.GetSection("JwtToken");
        var key = jwtSettings.GetSection("IssuerSigningKey").Value;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
}