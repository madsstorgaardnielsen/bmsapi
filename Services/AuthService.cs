using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BMSAPI.Services;

public class AuthService {
    private readonly UserManager<User> _userManager;
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private User? _user;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper,
        UserRepository userRepository) {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> UpdateUserPassword(ChangePasswordDTO changePasswordDTO, CancellationToken ct) {
        var validationResult = await AuthenticateUser(new LoginDTO
            {Username = changePasswordDTO.Username, Password = changePasswordDTO.Password});

        if (!validationResult) {
            return IdentityResult.Failed();
        }

        var user = await _userManager.FindByNameAsync(changePasswordDTO.Username);
        var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.Password,
            changePasswordDTO.NewPassword);

        await _userRepository.SaveAsync(ct);
        return result;
    }

    public async Task<IdentityResult> CreateUser(CreateUserDTO userDTO) {
        var user = _mapper.Map<User>(userDTO);

        var result = await _userManager.CreateAsync(user, userDTO.Password);
        if (!result.Succeeded) {
            return result;
        }

        var role = new List<string> {"User"};
        var roleResult = await _userManager.AddToRolesAsync(user, role);
        if (!roleResult.Succeeded) {
            return roleResult;
        }

        return result;
    }

    public async Task<bool> AttemptLogin(LoginDTO loginDTO) {
        return await AuthenticateUser(loginDTO);
    }

    private async Task<bool> AuthenticateUser(LoginDTO loginDTO) {
        _user = await _userManager.FindByNameAsync(loginDTO.Username);
        return (_user != null && await _userManager.CheckPasswordAsync(_user, loginDTO.Password));
    }
}