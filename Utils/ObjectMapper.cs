using AutoMapper;
using BMSAPI.Database.Models;
using BMSAPI.Models;

namespace BMSAPI.Utils;

public class ObjectMapper : Profile {
    public ObjectMapper() {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, CreateUserDTO>().ReverseMap();
        CreateMap<User, UpdateUserDTO>().ReverseMap();

        CreateMap<Child, ChildDTO>().ReverseMap();
        CreateMap<Child, CreateChildDTO>().ReverseMap();
        CreateMap<Child, UpdateChildDTO>().ReverseMap();
        CreateMap<Child, SimpleChildDTO>().ReverseMap();
        CreateMap<ChildDTO, SimpleChildDTO>().ReverseMap();

        CreateMap<Diaper, DiaperDTO>().ReverseMap();
        CreateMap<Diaper, CreateDiaperDTO>().ReverseMap();
        CreateMap<Diaper, UpdateDiaperDTO>().ReverseMap();
        
        CreateMap<Feeding, FeedingDTO>().ReverseMap();
        CreateMap<Feeding, CreateFeedingDTO>().ReverseMap();
        
        CreateMap<Measurement, MeasurementDTO>().ReverseMap();
        CreateMap<Measurement, CreateMeasurementDTO>().ReverseMap();
    }
}