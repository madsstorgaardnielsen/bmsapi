using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services;

public class ChildService {
    private readonly ChildRepository _childRepository;
    private readonly ILogger<ChildService> _logger;
    private readonly IMapper _mapper;

    public ChildService(ChildRepository childRepository,
        ILogger<ChildService> logger,
        IMapper mapper) {
        _childRepository = childRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<SimpleChildDTO?> AddChild(CreateChildDTO childDTO, CancellationToken ct) {
        var child = _mapper.Map<Child>(childDTO);
        var createdChild = await _childRepository.Create(child, ct);
        var result = await _childRepository.AddParentsToChild(createdChild.Id, childDTO, ct);
        return _mapper.Map<SimpleChildDTO>(result);
    }

    public async Task<List<SimpleChildDTO>> GetAllChildren(string username, CancellationToken ct) {
        var children = await _childRepository.GetAllByUsername(username, ct);
        return _mapper.Map<List<SimpleChildDTO>>(children).OrderBy(x => x.Birthdate).ToList();
    }

    public async Task<ChildDTO?> GetChild(string username, string childId, CancellationToken ct) {
        var child = await _childRepository.GetByChildId(username, childId, ct);
        return _mapper.Map<ChildDTO>(child);
    }

    public async Task<bool> DeleteChild(string childId, string username, CancellationToken ct) {
        return await _childRepository.Delete(childId, ct);
    }

    public async Task<ChildDTO?> UpdateChild(string username, UpdateChildDTO childDTO, CancellationToken ct) {
        var child = await _childRepository.GetByChildId(username, childDTO.Id, ct);

        if (child != null) {
            child.Name = childDTO.Name;
            child.Birthdate = childDTO.Birthdate;
            child.Parents[0].Id = childDTO.ParentOneId;

            if (childDTO.ParentTwoId != "") {
                child.Parents.Remove(child.Parents[1]);
            }
        }

        if (await _childRepository.SaveAsync(ct)) {
            return _mapper.Map<ChildDTO>(child);
        }

        return null;
    }
}