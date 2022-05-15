using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;
    private readonly JWTService _jwtService;


    public AuthController(AuthService authService, JWTService jwtService) {
        _authService = authService;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var result = await _authService.AttemptLogin(loginDTO);

        if (result) {
            return Accepted(new {Token = await _jwtService.CreateToken(loginDTO.Username)});
        }

        return Unauthorized("Wrong username or password");
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateUserDTO userDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var result = await _authService.CreateUser(userDto);

        if (result.Succeeded) {
            return Accepted();
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    [Authorize]
    [HttpPost("changepassword", Name = "ChangePassword")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDTO changePasswordDTO,
        CancellationToken ct) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var result = await _authService.UpdateUserPassword(changePasswordDTO, ct);

        if (result.Succeeded) {
            return NoContent();
        }

        return Unauthorized();
    }
}