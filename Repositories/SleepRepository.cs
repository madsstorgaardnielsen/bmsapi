using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public class SleepRepository : GenericRepository<Sleep, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public SleepRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    public async Task<List<Sleep>>
        GetAllSleeps(string username, GetAllSleepsDTO sleepsDTO, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x =>
                    x.Sleeps
                        .Where(y => y.ChildId == sleepsDTO.ChildId)
                        .Where(y => sleepsDTO.From == null || y.From >= sleepsDTO.From)
                        .Where(y => sleepsDTO.To == null || y.To <= sleepsDTO.To))
                .SingleOrDefaultAsync(ct);

        var sleeps =
            user?.Children
                .SingleOrDefault(x => x.Id == sleepsDTO.ChildId)
                ?.Sleeps
                .ToList();

        return sleeps == null ? new List<Sleep>() : sleeps.OrderBy(x => x.From).ToList();
    }
}