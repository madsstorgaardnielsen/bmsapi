// using BMSAPI.Models;
// using BMSAPI.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace BMSAPI.Controllers;
//
//
//
// [Route("api/[controller]")]
// [ApiController]
// public class SleepController : ControllerBase {
//     private readonly SleepService _sleepService;
//     private readonly ILogger<SleepController> _logger;
//
//     public SleepController(SleepService sleepService, ILogger<SleepController> logger) {
//         _sleepService = sleepService;
//         _logger = logger;
//     }
//
//     [Authorize]
//     [HttpPost(Name = "AddSleep")]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> AddSleep([FromBody] CreateSleepDTO sleepDTO, CancellationToken ct) {
//         if (!ModelState.IsValid) {
//             _logger.LogError($"Error validating data in {nameof(AddSleep)}");
//             return BadRequest(ModelState);
//         }
//
//         var result = await _sleepService.AddSleep(User.Identity.Name, sleepDTO, ct);
//         if (result != null) {
//             return Ok();
//         }
//
//         return Problem("Error adding sleep");
//     }
//
//     [Authorize]
//     [HttpDelete("{sleepId}")]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> DeleteSleep(string sleepId, CancellationToken ct) {
//         var result = await _sleepService.DeleteSleep(sleepId, User.Identity.Name, ct);
//         if (result) {
//             return NoContent();
//         }
//
//         _logger.LogInformation($"Error deleting diaper with id: {sleepId}");
//         return BadRequest();
//     }
//
//     [Authorize]
//     [HttpGet(Name = "GetAllFeedings")]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetAllSleeps([FromBody] SleepDTO sleepDTO, CancellationToken ct) {
//         var result = await _sleepService.GetAllSleeps(User.Identity.Name, sleepDTO, ct);
//         if (result.Count > 0 && result != null) {
//             return Ok(result);
//         }
//
//         return NotFound($"No feedings for child with id: {sleepDTO.ChildId} found");
//     }
// }
//
//
//
//
// //get daily average over period