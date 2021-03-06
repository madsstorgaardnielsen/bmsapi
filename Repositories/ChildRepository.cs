using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BMSAPI.Repositories;

public class ChildRepository : GenericRepository<Child, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public ChildRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    public async Task<Child?> AddParentsToChild(string childId, CreateChildDTO childDTO, CancellationToken ct) {
        var child = await
            _dbContext
                .Children
                .Include(x => x.Parents)
                .SingleOrDefaultAsync(x => x.Id == childId, ct);

        var parent1 = await
            _dbContext
                .Users
                .SingleOrDefaultAsync(x => x.Id == childDTO.ParentOneId, ct);

        if (parent1 != null && child != null) {
            child.Parents.Add(parent1);
        }

        if (!childDTO.ParentTwoId.IsNullOrEmpty()) {
            var parent2 = await
                _dbContext
                    .Users
                    .SingleOrDefaultAsync(x => x.Id == childDTO.ParentTwoId, ct);

            if (parent2 != null && child != null) {
                child.Parents.Add(parent2);
            }
        }

        var success = await _dbContext.SaveChangesAsync(ct) > 0;

        return success ? child : null;
    }

    public async Task<List<Child>> GetAllByUsername(string username, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x => x.Parents)
                .SingleOrDefaultAsync(ct);

        var children = user?.Children.ToList();

        return children == null ? new List<Child>() : children.OrderBy(x => x.Birthdate).ToList();
    }

    public async Task<Child?> GetByChildId(string username, string childId, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x =>
                    x.Children
                        .Where(y => y.Id == childId))
                .ThenInclude(x => x.Parents)
                .ThenInclude(x => x.FeedingProfiles)
                .SingleOrDefaultAsync(ct);

        var child = user?
            .Children
            .SingleOrDefault(x => x.Id == childId);

        return child;
    }
}