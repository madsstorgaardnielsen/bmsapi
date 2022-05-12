using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly JWTService _jwtService;
    private readonly UserRepository _userRepository;

    public AuthController(UserManager<User> userManager, ILogger<AuthController> logger, IMapper mapper,
        IAuthService authService, JWTService jwtService, UserRepository userRepository) {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _authService = authService;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {
        _logger.LogInformation("Init login attempt");
        _logger.LogInformation("Username: " + loginDTO.Username);
        _logger.LogInformation("------------------");

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var validationResult = await _authService.AuthenticateUser(loginDTO);

        if (validationResult) {
            return Accepted(new {Token = await _jwtService.CreateToken(loginDTO.Username)});
        }

        return Unauthorized("Wrong username or password");
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateUserDTO userDto) {
        _logger.LogInformation($"Init registration attempt: {userDto.Email}");

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userDto);
        // user.UserName = userDto.Email.Split("@")[0];

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded) {
            var role = new List<string> {"User"};
            await _userManager.AddToRolesAsync(user, role);
            return Accepted();
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError(error.Code, error.Description);
            _logger.LogInformation($"Error code: {error.Code}");
            _logger.LogInformation($"Error description: {error.Description}");
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

        if (ModelState.IsValid) {
            var validationResult = await _authService.AuthenticateUser(new LoginDTO
                {Username = changePasswordDTO.Username, Password = changePasswordDTO.Password});

            if (validationResult) {
                var user = await _userManager.FindByNameAsync(changePasswordDTO.Username);
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.Password,
                    changePasswordDTO.NewPassword);
                if (result.Succeeded) {
                    await _userRepository.SaveAsync(ct);
                    return NoContent();
                }
            }
        }

        return Unauthorized();
    }
}