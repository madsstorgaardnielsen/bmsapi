using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Repositories;

public class UserRepository : GenericRepository<User, DatabaseContext> {
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }

    
}