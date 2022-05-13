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

    public async Task<ChildDTO?> AddParentsToChild(string childId, CreateChildDTO childDTO, CancellationToken ct) {
        var child = await _dbContext.Children.Include(x => x.Parents).SingleOrDefaultAsync(x => x.Id == childId, ct);

        var parent1 = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == childDTO.ParentOneId, ct);

        if (parent1 != null && child != null) {
            child.Parents.Add(parent1);
        }

        if (!childDTO.ParentTwoId.IsNullOrEmpty()) {
            var parent2 = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == childDTO.ParentTwoId, ct);
            if (parent2 != null && child != null) {
                child.Parents.Add(parent2);
            }
        }

        var success = await _dbContext.SaveChangesAsync(ct) > 0;

        if (success) {
            var result = _mapper.Map<ChildDTO>(child);
            return result;
        }

        return null;
    }
}