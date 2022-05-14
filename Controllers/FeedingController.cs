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

    //AddFeeding
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

        var result = await _feedingService.AddFeeding(User.Identity.Name, feedingDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding diaper");
    }
    
    //Delete feeding
    [Authorize]
    [HttpDelete("{feedingId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeeding(string feedingId, CancellationToken ct) {
        var result = await _feedingService.DeleteFeeding(feedingId, User.Identity.Name, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting diaper with id: {feedingId}");
        return BadRequest();
    }
    
    //get all feedings
    [Authorize]
    [HttpGet( Name = "GetAllFeedings")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllFeedings([FromBody] GetAllFeedingDTO feedingDTO, CancellationToken ct) {
        var result = await _feedingService.GetAllFeedings(User.Identity.Name, feedingDTO, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound($"No feedings for child with id: {feedingDTO.ChildId} found");
    }
    
    
    
    
    //get daily average over period
}