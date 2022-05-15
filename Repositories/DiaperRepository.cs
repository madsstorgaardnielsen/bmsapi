using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public class DiaperRepository : GenericRepository<Diaper, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public DiaperRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }

    public async Task<Diaper?> GetDiaper(string username, string diaperId, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x =>
                    x.Diapers
                        .Where(y => y.Id == diaperId))
                .SingleOrDefaultAsync(ct);

        if (user == null) {
            return null;
        }

        var diaper = await
            _dbContext
                .Diapers
                .Include(x => x.Child)
                .ThenInclude(x => x.Parents)
                .Where(x => x.Id == diaperId)
                .Where(x => x.Child.Parents.Contains(user))
                .SingleOrDefaultAsync(ct);

        return diaper ?? null;
    }

    public async Task<List<Diaper>> GetAllDiapers(string username, GetAllDiapersDTO diapersDTO, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x =>
                    x.Diapers
                        .Where(y => y.ChildId == diapersDTO.ChildId)
                        .Where(y => diapersDTO.FromDate == null || y.DateTime >= diapersDTO.FromDate)
                        .Where(y => diapersDTO.ToDate == null || y.DateTime <= diapersDTO.ToDate))
                .SingleOrDefaultAsync(ct);

        var diapers = user?.Children.SingleOrDefault(x => x.Id == diapersDTO.ChildId)?.Diapers.ToList();

        return diapers == null ? new List<Diaper>() : diapers.OrderBy(x => x.DateTime).ToList();
    }
}