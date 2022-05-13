using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiaperController : ControllerBase {
    private readonly DiaperRepository _diaperRepository;
    private readonly ChildRepository _childRepository;
    private readonly ILogger<DiaperController> _logger;
    private readonly IMapper _mapper;

    public DiaperController(DiaperRepository diaperRepository, ChildRepository childRepository,
        ILogger<DiaperController> logger, IMapper mapper) {
        _diaperRepository = diaperRepository;
        _logger = logger;
        _mapper = mapper;
        _childRepository = childRepository;
    }

    public async Task<IActionResult> AddDiaper([FromBody] CreateDiaperDTO diaperDTO, CancellationToken ct) {
        if (!ModelState.IsValid) {
            _logger.LogError($"Error validating data in {nameof(AddDiaper)}");
            return BadRequest(ModelState);
        }

        var mappedDiaper = _mapper.Map<Diaper>(diaperDTO);
        var diaper = await _diaperRepository.Create(mappedDiaper, ct);

        if (diaper != null) {
            var child = await _childRepository.GetByChildId(User.Identity.Name, diaperDTO.ChildId, ct);
            diaper.Child = child;
            await _diaperRepository.SaveAsync(ct);
        }

        return Problem("Error adding diaper");
    }
}