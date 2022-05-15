using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Repositories;

public class UserRepository : GenericRepository<User, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    
}