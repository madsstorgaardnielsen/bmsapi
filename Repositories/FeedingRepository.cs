using System.Linq;
using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BMSAPI.Repositories;

public class FeedingRepository : GenericRepository<Feeding, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public FeedingRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    private async Task<User?> GetUser(string username, CancellationToken ct) {
        return await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .SingleOrDefaultAsync(ct);
    }

    public async Task<bool> DeleteFeedingProfile(string username,
        string profileId, CancellationToken ct) {
        var user = await GetUser(username, ct);
        var profile = await _dbContext.FeedingProfiles.Where(x => x.User == user && x.Id == profileId)
            .SingleOrDefaultAsync(ct);
        if (profile != null) {
            _dbContext.FeedingProfiles.Remove(profile);
            return true;
        }

        return false;
    }

    public async Task<FeedingProfile?> GetFeedingProfile(string username,
        string profileId, CancellationToken ct) {
        var user = await GetUser(username, ct);
        return await _dbContext.FeedingProfiles.Where(x => x.User == user && x.Id == profileId).AsNoTracking()
            .SingleOrDefaultAsync(ct);
    }

    public async Task<FeedingProfile> CreateFeedingProfile(string username,
        FeedingProfile createFeedingProfile, CancellationToken ct) {
        var user = await GetUser(username, ct);
        createFeedingProfile.User = user;
        _dbContext.FeedingProfiles.Add(createFeedingProfile);
        return createFeedingProfile;
    }

    public async Task<List<FeedingProfile>>
        GetAllFeedingProfiles(string username, CancellationToken ct) {
        var user = await GetUser(username, ct);

        var feedingProfiles = await _dbContext.FeedingProfiles.Where(x => x.User == user).ToListAsync(ct);

        return feedingProfiles.OrderBy(x => x.Title).ToList();
    }

    public async Task<List<Feeding>>
        GetAllFeedings(string username, GetAllFeedingDTO feedingDTO, CancellationToken ct) {
        var user = await GetUser(username, ct);

        var feedings = await
            _dbContext
                .Feedings
                .Include(x => x.Child)
                .Include(x => x.Child.Parents)
                .Where(x =>
                    (feedingDTO.From == null || feedingDTO.To == null) ||
                    (x.DateTime >= feedingDTO.From && x.DateTime <= feedingDTO.To))
                .Where(x => x.Child.Id == feedingDTO.ChildId)
                .Where(x => x.Child.Parents.Contains(user))
                .ToListAsync(ct);

        return feedings.OrderBy(x => x.DateTime).ToList();
    }
}