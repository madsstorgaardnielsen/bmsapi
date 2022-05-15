using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public class FeedingRepository : GenericRepository<Feeding, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public FeedingRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    public async Task<Feeding?> GetFeeding(string username, string feedingId, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x =>
                    x.Feedings
                        .Where(y => y.Id == feedingId))
                .SingleOrDefaultAsync(ct);

        if (user == null) {
            return null;
        }

        var feeding = await
            _dbContext
                .Feedings
                .Include(x => x.Child)
                .ThenInclude(x => x.Parents)
                .Where(x => x.Id == feedingId)
                .Where(x => x.Child.Parents.Contains(user))
                .SingleOrDefaultAsync(ct);

        return feeding ?? null;
    }

    public async Task<bool> DeleteFeedingProfile(string username,
        string profileId, CancellationToken ct) {
        var profile = await
            _dbContext
                .FeedingProfiles
                .Include(x => x.User)
                .Where(x => x.User.UserName == username)
                .Where(x => x.Id == profileId)
                .SingleOrDefaultAsync(ct);

        if (profile != null) {
            _dbContext.FeedingProfiles.Remove(profile);
            return true;
        }

        return false;
    }

    public async Task<FeedingProfile?> GetFeedingProfile(string username,
        string profileId, CancellationToken ct) {
        return await _dbContext
            .FeedingProfiles
            .Include(x => x.User)
            .Where(x => x.User.UserName == username)
            .Where(x => x.Id == profileId)
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);
    }

    public async Task<FeedingProfile> CreateFeedingProfile(string username,
        FeedingProfile createFeedingProfile, CancellationToken ct) {
        var user = await _dbContext
            .Users
            .Where(x => x.UserName == username)
            .Include(x => x.Children)
            .SingleOrDefaultAsync(ct);

        if (user == null) {
            return new FeedingProfile();
        }

        createFeedingProfile.User = user;
        _dbContext.FeedingProfiles.Add(createFeedingProfile);
        return createFeedingProfile;
    }

    public async Task<List<FeedingProfile>>
        GetAllFeedingProfiles(string username, CancellationToken ct) {
        var feedingProfiles = await
            _dbContext
                .FeedingProfiles
                .Include(x => x.User)
                .Where(x => x.User.UserName == username)
                .ToListAsync(ct);

        return feedingProfiles.OrderBy(x => x.Title).ToList();
    }

    public async Task<List<Feeding>>
        GetAllFeedings(string username, GetAllFeedingDTO feedingDTO, CancellationToken ct) {
        var user = await _dbContext
            .Users
            .Where(x => x.UserName == username)
            .Include(x => x.Children)
            .ThenInclude(y =>
                y.Feedings
                    .Where(x => x.ChildId == feedingDTO.ChildId)
                    .Where(x => feedingDTO.From == null || x.DateTime >= feedingDTO.From)
                    .Where(x => feedingDTO.To == null || x.DateTime <= feedingDTO.To))
            .SingleOrDefaultAsync(ct);

        var feedings =
            user?
                .Children
                .SingleOrDefault(x => x.Id == feedingDTO.ChildId)?
                .Feedings
                .ToList();

        return feedings == null ? new List<Feeding>() : feedings.OrderBy(x => x.DateTime).ToList();
    }
}