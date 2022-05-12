using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using BMSAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;

    public UserController(UserRepository userRepository, ILogger<UserController> logger, IMapper mapper) {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPut(Name = "UpdateUser")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO userDTO, CancellationToken ct) {
        if (ModelState.IsValid) {
            var user = await _userRepository.Get(userDTO.Id, ct);

            user.Name = userDTO.Name;
            user.Lastname = userDTO.Lastname;
            user.Country = userDTO.Country;
            user.City = userDTO.City;
            user.Zip = userDTO.Zip;
            user.Street = userDTO.Street;
            user.StreetNumber = userDTO.StreetNumber;
            user.Email = userDTO.Email;
            user.Floor = userDTO.Floor;

            var result = await _userRepository.SaveAsync(ct);

            if (result) {
                var mappedResult = _mapper.Map<UserDTO>(result);
                return Ok(mappedResult);
            }
        }
        else {
            _logger.LogError($"Error validating data in {nameof(UpdateUser)}");
            return BadRequest(ModelState);
        }

        return Problem("Error updating user");
    }
}