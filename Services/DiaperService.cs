using AutoMapper;
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

    public async Task<List<DiaperDTO>> GetAllDiapers(string username, GetAllDiapersDTO diapersDTO, CancellationToken ct) {
        var diapers = await _diaperRepository.GetAllDiapers(username, diapersDTO, ct);
        return _mapper.Map<List<DiaperDTO>>(diapers);
    }
    
    public async Task<bool> DeleteDiaper(string diaperId, string username, CancellationToken ct) {
        return await _diaperRepository.Delete(diaperId, ct);
    }
}