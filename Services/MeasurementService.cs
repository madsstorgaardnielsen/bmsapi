using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services;

public class MeasurementService {
    private readonly MeasurementRepository _measurementRepository;
    private readonly ChildRepository _childRepository;
    private readonly ILogger<MeasurementService> _logger;
    private readonly IMapper _mapper;

    public MeasurementService(ChildRepository childRepository, MeasurementRepository measurementRepository,
        ILogger<MeasurementService> logger,
        IMapper mapper) {
        _childRepository = childRepository;
        _measurementRepository = measurementRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<MeasurementDTO?> AddMeasurement(string username, CreateMeasurementDTO measurementDTO,
        CancellationToken ct) {
        var mappedMeasurement = _mapper.Map<Measurement>(measurementDTO);
        var measurement = await _measurementRepository.Create(mappedMeasurement, ct);

        if (measurement != null) {
            var child = await _childRepository.GetByChildId(username, measurementDTO.ChildId, ct);
            if (child != null) {
                measurement.Child = child;
                await _measurementRepository.SaveAsync(ct);
            }
        }

        return _mapper.Map<MeasurementDTO>(measurement);
    }

    public async Task<List<MeasurementDTO>> GetAllMeasurements(string username, string childId, CancellationToken ct) {
        var diapers = await _measurementRepository.GetAllMeasurementsByUsername(username, childId, ct);
        return _mapper.Map<List<MeasurementDTO>>(diapers);
    }

    public async Task<bool> DeleteMeasurement(string feedingId, string username, CancellationToken ct) {
        return await _measurementRepository.Delete(feedingId, ct);
    }
}