using BMSAPI.Models;

namespace BMSAPI.Services; 

public interface IAuthService {
    Task<bool> AuthenticateUser(LoginDTO loginDTO);
}