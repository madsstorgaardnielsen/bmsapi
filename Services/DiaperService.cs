using AutoMapper;
using BMSAPI.Controllers;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services;

public class DiaperService {
    private readonly DiaperRepository _diaperRepository;
    private readonly ChildRepository _childRepository;
    private readonly ILogger<DiaperService> _logger;
    private readonly IMapper _mapper;

    public DiaperService(DiaperRepository diaperRepository, ChildRepository childRepository,
        ILogger<DiaperService> logger, IMapper mapper) {
        _diaperRepository = diaperRepository;
        _logger = logger;
        _mapper = mapper;
        _childRepository = childRepository;
    }

    public async Task<DiaperDTO?> AddDiaper(string username, CreateDiaperDTO diaperDTO, CancellationToken ct) {
        var mappedDiaper = _mapper.Map<Diaper>(diaperDTO);
        var diaper = await _diaperRepository.Create(mappedDiaper, ct);

        if (diaper != null) {
            var child = await _childRepository.GetByChildId(username, diaperDTO.ChildId, ct);
            if (child != null) {
                diaper.Child = child;
                await _diaperRepository.SaveAsync(ct);
            }
        }

        return _mapper.Map<DiaperDTO>(diaper);
    }
}