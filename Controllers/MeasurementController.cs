using BMSAPI.Models;
using BMSAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers; 



[Route("api/[controller]")]
[ApiController]
public class MeasurementController : ControllerBase {
    private readonly MeasurementService _measurementService;
    private readonly ILogger<MeasurementController> _logger;
    
    public MeasurementController(MeasurementService measurementService, ILogger<MeasurementController> logger) {
        _measurementService = measurementService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost(Name = "AddMeasurement")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddMeasurement([FromBody] CreateMeasurementDTO measurementDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddMeasurement)}");
            return BadRequest(ModelState);
        }

        var result = await _measurementService.AddMeasurement(User.Identity.Name, measurementDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding measurement");
    }
    
    [Authorize]
    [HttpDelete("{measurementId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMeasurement(string measurementId, CancellationToken ct) {
        var result = await _measurementService.DeleteMeasurement(measurementId, User.Identity.Name, ct);
        if (result) {
            return NoContent();
        }

        _logger.LogInformation($"Error deleting measurement with id: {measurementId}");
        return BadRequest();
    }
    
    [Authorize]
    [HttpGet("{childId}", Name = "GetAllMeasurements")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMeasurements(string childId, CancellationToken ct) {
        var result = await _measurementService.GetAllMeasurements(User.Identity.Name, childId, ct);
        if (result.Count > 0 && result != null) {
            return Ok(result);
        }

        return NotFound($"No measurements for child with id: {childId} found");
    }
    
    
    
    
    //avg height growth
    //avg weight gain
    //etc
}