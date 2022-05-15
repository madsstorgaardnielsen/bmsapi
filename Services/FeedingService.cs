using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services;

public class FeedingService {
    private readonly ILogger<FeedingService> _logger;
    private readonly IMapper _mapper;
    private readonly FeedingRepository _feedingRepository;
    private readonly ChildRepository _childRepository;

    public FeedingService(FeedingRepository feedingRepository, ChildRepository childRepository,
        ILogger<FeedingService> logger, IMapper mapper) {
        _logger = logger;
        _mapper = mapper;
        _feedingRepository = feedingRepository;
        _childRepository = childRepository;
    }

    public async Task<bool> DeleteFeedingProfile(string profileId, string username, CancellationToken ct) {
        await _feedingRepository.DeleteFeedingProfile(username, profileId, ct);
        return await _feedingRepository.SaveAsync(ct);
    }

    public async Task<List<SimpleFeedingProfileDTO>> GetAllFeedingProfiles(string username,
        CancellationToken ct) {
        var feedingProfiles = await _feedingRepository.GetAllFeedingProfiles(username, ct);
        return _mapper.Map<List<SimpleFeedingProfileDTO>>(feedingProfiles);
    }


    public async Task<ChildFeedingProfileDTO?> SetFeedingProfile(string username,
        SetFeedingProfileDTO setFeedingProfileDTO,
        CancellationToken ct) {
        var child = await _childRepository.GetByChildId(username, setFeedingProfileDTO.ChildId, ct);
        var feedingProfile = await _feedingRepository.GetFeedingProfile(username, setFeedingProfileDTO.ProfileId, ct);
        if (feedingProfile != null && child != null) {
            child.FeedingProfile = feedingProfile;

            await _childRepository.SaveAsync(ct);

            var profileDTO = _mapper.Map<SimpleFeedingProfileDTO>(feedingProfile);
            var childDTO = _mapper.Map<SimpleChildDTO>(child);
            return new ChildFeedingProfileDTO {Child = childDTO, FeedingProfile = profileDTO};
        }

        return null;
    }


    public async Task<FeedingProfileDTO?> CreateFeedingProfile(string username, CreateFeedingProfileDTO feedingDTO,
        CancellationToken ct) {
        var mappedFeedingProfile = _mapper.Map<FeedingProfile>(feedingDTO);
        var feeding = await _feedingRepository.CreateFeedingProfile(username, mappedFeedingProfile, ct);

        await _feedingRepository.SaveAsync(ct);

        return _mapper.Map<FeedingProfileDTO>(feeding);
    }

    public async Task<FeedingDTO?> AddFeeding(string username, CreateFeedingDTO feedingDTO, CancellationToken ct) {
        var mappedFeeding = _mapper.Map<Feeding>(feedingDTO);
        var feeding = await _feedingRepository.Create(mappedFeeding, ct);

        if (feeding != null) {
            var child = await _childRepository.GetByChildId(username, feedingDTO.ChildId, ct);
            if (child != null) {
                feeding.Child = child;
                await _feedingRepository.SaveAsync(ct);
            }
        }

        return _mapper.Map<FeedingDTO>(feeding);
    }

    public async Task<List<FeedingDTO>> GetAllFeedings(string username, GetAllFeedingDTO feedingDTO,
        CancellationToken ct) {
        var diapers = await _feedingRepository.GetAllFeedings(username, feedingDTO, ct);
        return _mapper.Map<List<FeedingDTO>>(diapers);
    }

    public async Task<bool> DeleteFeeding(string feedingId, string username, CancellationToken ct) {
        return await _feedingRepository.Delete(feedingId, ct);
    }
}