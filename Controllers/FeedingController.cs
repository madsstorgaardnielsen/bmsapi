using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedingController : ControllerBase {
    private readonly FeedingService _feedingService;
    private readonly ILogger<FeedingController> _logger;

    public FeedingController(FeedingService feedingService, ILogger<FeedingController> logger) {
        _feedingService = feedingService;
        _logger = logger;
    }

    //get average daily intake for period
    //get how much food is needed according to feeding profile
        //calculate difference between total amount from feeding profile and sum of each feeding amount from date

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

    [Authorize]
    [HttpPost(Name = "AddFeeding")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFeeding([FromBody] CreateFeedingDTO feedingDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddFeeding)}");
            return BadRequest(ModelState);
        }

        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.AddFeeding(user, feedingDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding feeding");
    }

    [Authorize]
    [HttpDelete("{feedingId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeeding(string feedingId, CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.DeleteFeeding(feedingId, user, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting diaper with id: {feedingId}");
        return BadRequest();
    }

    [Authorize]
    [HttpGet("feedings/{childId}", Name = "GetAllFeedings")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllFeedings([FromBody] GetAllFeedingDTO feedingDTO, string childId,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        feedingDTO.ChildId = childId;
        var result = await _feedingService.GetAllFeedings(user, feedingDTO, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No feedings for child with id: {feedingDTO.ChildId} found in the period: {feedingDTO.From} to {feedingDTO.To}");
    }


}