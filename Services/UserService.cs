using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;

namespace BMSAPI.Services;

public class UserService {
    private readonly ChildRepository _childRepository;
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(ChildRepository childRepository, UserRepository userRepository,
        ILogger<UserService> logger,
        IMapper mapper) {
        _childRepository = childRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserDTO?> UpdateUser(string username, UpdateUserDTO userDTO, CancellationToken ct) {
        var user = await _userRepository.Get(userDTO.Id, ct);

        if (user != null) {
            user.UserName = userDTO.UserName;
            user.Email = userDTO.Email;
            user.Name = userDTO.Name;
            user.Lastname = userDTO.Lastname;
            user.Country = userDTO.Country;
            user.City = userDTO.City;
            user.Zip = userDTO.Zip;
            user.Street = userDTO.Street;
            user.StreetNumber = userDTO.StreetNumber;
            user.Floor = userDTO.Floor;
            user.PhoneNumber = userDTO.PhoneNumber;

            var result = await _userRepository.SaveAsync(ct);

            if (result) {
                return _mapper.Map<UserDTO>(user);
            }
        }

        return null;
    }

    public async Task<SimpleChildDTO?> AddChild(CreateChildDTO childDTO, CancellationToken ct) {
        var child = _mapper.Map<Child>(childDTO);
        var createdChild = await _childRepository.Create(child, ct);
        var result = await _userRepository.AddParentsToChild(createdChild.Id, childDTO, ct);
        return _mapper.Map<SimpleChildDTO>(result);
    }
}