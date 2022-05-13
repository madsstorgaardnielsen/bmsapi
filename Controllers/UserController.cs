using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;
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
    [HttpPost(Name = "AddChild")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddChild([FromBody] CreateChildDTO childDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddChild)}");
            return BadRequest(ModelState);
        }

        var child = await _userService.AddChild(childDTO, ct);

        if (child != null) {
            return CreatedAtAction(nameof(AddChild), new {id = child.Id}, child);
        }


        return Problem("Error adding child");
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