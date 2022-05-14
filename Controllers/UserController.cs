using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(UserService userService,
        ILogger<UserController> logger) {
        _logger = logger;
        _userService = userService;
    }

    

    [Authorize]
    [HttpPut(Name = "UpdateUser")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO userDTO, CancellationToken ct) {
        if (ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(UpdateUser)}");
            return BadRequest(ModelState);
        }

        var result = await _userService.UpdateUser(User.Identity.Name, userDTO, ct);
        if (result != null) {
            return Ok(result);
        }


        return Problem("Error updating user");
    }
}