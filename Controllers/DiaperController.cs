using BMSAPI.Models;
using BMSAPI.Services;
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

    public async Task<IActionResult> AddDiaper([FromBody] CreateDiaperDTO diaperDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddDiaper)}");
            return BadRequest(ModelState);
        }

        var result = await _diaperService.AddDiaper(User.Identity.Name, diaperDTO, ct);
        if (result != null) {
            return Ok();
        }

        return Problem("Error adding diaper");
    }
}