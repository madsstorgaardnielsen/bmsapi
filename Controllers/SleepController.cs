using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SleepController : ControllerBase {
    private readonly SleepService _sleepService;
    private readonly ILogger<SleepController> _logger;

    public SleepController(SleepService sleepService, ILogger<SleepController> logger) {
        _sleepService = sleepService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("average", Name = "GetDailyAverageTimeSlept")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDailyAverageTimeSlept([FromBody] GetAllSleepsDTO sleepsDTO,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _sleepService.GetDailyAverageTimeSlept(user, sleepsDTO, ct);
        if (result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No sleeps for child with id: {sleepsDTO.ChildId} found in the period: {sleepsDTO.From} to {sleepsDTO.To}");
    }

    [Authorize]
    [HttpGet("status/{childId}", Name = "GetDailySleepStatus")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDailySleepStatus(string childId,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _sleepService.GetDailyStatus(user, childId, ct);
        if (result != null) {
            return Ok(result);
        }

        return NotFound(
            $"No sleeps for child with id: {childId} found");
    }

    [Authorize]
    [HttpPost(Name = "AddSleep")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddSleep([FromBody] CreateSleepDTO sleepDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddSleep)}");
            return BadRequest(ModelState);
        }

        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _sleepService.AddSleep(user, sleepDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding sleep");
    }

    [Authorize]
    [HttpDelete("{sleepId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSleep(string sleepId, CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        var result = await _sleepService.DeleteSleep(sleepId, user, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting diaper with id: {sleepId}");
        return BadRequest();
    }

    [Authorize]
    [HttpGet("sleeps/{childId}", Name = "GetAllSleeps")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSleeps([FromBody] GetAllSleepsDTO sleepDTO, string childId,
        CancellationToken ct) {
        var user = User.Identity!.Name;
        if (user == null) {
            return Unauthorized();
        }

        sleepDTO.ChildId = childId;
        var result = await _sleepService.GetAllSleeps(user, sleepDTO, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound($"No feedings for child with id: {sleepDTO.ChildId} found");
    }
}