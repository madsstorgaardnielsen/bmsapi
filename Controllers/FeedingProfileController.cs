using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/Feeding")]
[ApiController]
public class FeedingProfileController : ControllerBase {
    private readonly FeedingService _feedingService;
    private readonly ILogger<FeedingProfileController> _logger;

    public FeedingProfileController(FeedingService feedingService, ILogger<FeedingProfileController> logger) {
        _feedingService = feedingService;
        _logger = logger;
    }

    [Authorize]
    [HttpDelete("profile/{profileId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeedingProfile(string profileId, CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.DeleteFeedingProfile(profileId, user, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting feeding profile with id: {profileId}");
        return BadRequest();
    }

    [Authorize]
    [HttpGet("profiles", Name = "GetAllFeedingProfiles")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllFeedingProfiles(
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.GetAllFeedingProfiles(user, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No feeding profiles found");
    }


    [Authorize]
    [HttpPost("profile/set", Name = "SetFeedingProfile")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetFeedingProfile([FromBody] SetFeedingProfileDTO setFeedingProfileDTO,
        CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(SetFeedingProfile)}");
            return BadRequest(ModelState);
        }

        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.SetFeedingProfile(user, setFeedingProfileDTO, ct);
        if (result != null) {
            return Ok(result);
        }

        return Problem(
            $"Error setting feeding profile with id: {setFeedingProfileDTO.ProfileId} for child with id: {setFeedingProfileDTO.ChildId}");
    }

    [Authorize]
    [HttpPost("profile", Name = "CreateFeedingProfile")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateFeedingProfile([FromBody] CreateFeedingProfileDTO feedingDTO,
        CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(CreateFeedingProfile)}");
            return BadRequest(ModelState);
        }

        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.CreateFeedingProfile(user, feedingDTO, ct);
        if (result != null) {
            return Ok(new {id = result.Id});
        }

        return Problem("Error adding feeding profile");
    }
}