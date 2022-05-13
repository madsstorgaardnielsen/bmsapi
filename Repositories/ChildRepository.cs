using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Repositories;

public class ChildRepository : GenericRepository<Child, DatabaseContext> {
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public ChildRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<Child>> GetAllByUsername(string username, CancellationToken ct) {
        var user = await _dbContext.Users.Where(x => x.UserName == username).Include(x => x.Children)
            .SingleOrDefaultAsync(ct);

        var children = await _dbContext.Children.Where(x => x.Parents.Contains(user))
            .Include(x => x.Parents).ToListAsync(ct);

        return children.OrderBy(x => x.Birthdate).ToList();
    }

    public async Task<Child?> GetByChildId(string username, string childId, CancellationToken ct) {
        var user = await _dbContext.Users.Where(x => x.UserName == username).Include(x => x.Children)
            .SingleOrDefaultAsync(ct);

        var child = await _dbContext.Children.Where(x => x.Parents.Contains(user) && x.Id == childId)
            .Include(x => x.Parents).SingleOrDefaultAsync(ct);

        return child;
    }
}