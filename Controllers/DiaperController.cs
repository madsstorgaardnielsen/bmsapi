using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiaperController : ControllerBase {
    private readonly ILogger<DiaperController> _logger;
    private readonly DiaperService _diaperService;

    public DiaperController(ILogger<DiaperController> logger, DiaperService diaperService) {
        _logger = logger;
        _diaperService = diaperService;
    }

    [Authorize]
    [HttpPost(Name = "AddDiaper")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDiaper([FromBody] CreateDiaperDTO diaperDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddDiaper)}");
            return BadRequest(ModelState);
        }
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _diaperService.AddDiaper(user, diaperDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding diaper");
    }

    //TODO childid i url, from/to i body
    [Authorize]
    [HttpGet("diapers/{childId}", Name = "GetAllDiapers")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllDiapers([FromBody] GetAllDiapersDTO diapersDTO, string childId,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }
        diapersDTO.ChildId = childId;
        var result = await _diaperService.GetAllDiapers(user, diapersDTO, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound($"No diapers for child with id: {diapersDTO.ChildId} found");
    }

    [Authorize]
    [HttpDelete("{diaperId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDiaper(string diaperId, CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }
        var result = await _diaperService.DeleteDiaper(diaperId, user, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting diaper with id: {diaperId}");
        return BadRequest();
    }
}