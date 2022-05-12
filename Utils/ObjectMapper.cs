using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;

namespace BMSAPI.Utils; 

public class ObjectMapper : Profile{
    public ObjectMapper() {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, CreateUserDTO>().ReverseMap();
        CreateMap<User, UpdateUserDTO>().ReverseMap();
    }
}