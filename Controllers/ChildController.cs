using AutoMapper;
using BMSAPI.Models;
using BMSAPI.Repositories;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChildController : ControllerBase {
    private readonly ILogger<ChildController> _logger;
    private readonly ChildService _childService;

    public ChildController(
        ILogger<ChildController> logger, ChildService childService) {
        _childService = childService;
        _logger = logger;
    }

    [Authorize]
    [HttpDelete("delete/{childId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteChild(string childId, CancellationToken ct) {
        var result = await _childService.DeleteChild(childId, User.Identity.Name, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting child with id: {childId}");
        return BadRequest();
    }

    [Authorize]
    [HttpPut(Name = "UpdateChild")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateChild([FromBody] UpdateChildDTO childDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(UpdateChild)}");
            return BadRequest(ModelState);
        }

        var result = await _childService.UpdateChild(User.Identity.Name, childDTO, ct);

        if (result != null) {
            return Ok(result);
        }


        return Problem("Error updating child");
    }

    [Authorize]
    [HttpGet("{childId}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string childId, CancellationToken ct) {
        var result = await _childService.GetChild(User.Identity.Name, childId, ct);
        if (result != null) {
            return Ok(result);
        }

        return NotFound($"Child with id {childId} not found");
    }

    [Authorize]
    [HttpGet(Name = "GetAll")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken ct) {
        var result = await _childService.GetAllChildren(User.Identity.Name, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound("No children found");
    }
}