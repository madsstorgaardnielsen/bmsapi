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
    [HttpGet("average", Name = "GetAverageIntake")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAverageIntake([FromBody] GetAllFeedingDTO feedingDTO,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.GetAverageIntake(user, feedingDTO, ct);
        if (result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No feedings for child with id: {feedingDTO.ChildId} found in the period: {feedingDTO.From} to {feedingDTO.To}");
    }

    [Authorize]
    [HttpGet("status/{childId}", Name = "GetDailyStatus")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDailyStatus(string childId,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _feedingService.GetDailyStatus(user, childId, ct);
        if (result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No feedings for child with id: {childId} found");
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