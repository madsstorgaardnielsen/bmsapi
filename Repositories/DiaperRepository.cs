using AutoMapper;
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

    public async Task<List<Diaper>> GetAllDiapers(string username, GetAllDiapersDTO diapersDTO, CancellationToken ct) {
        
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .SingleOrDefaultAsync(ct);

        var diapers = await
            _dbContext
                .Diapers
                .Include(x => x.Child)
                .Where(x => x.ChildId == diapersDTO.ChildId)
                .Where(x =>
                    (diapersDTO.FromDate == null || diapersDTO.ToDate == null) ||
                    (x.DateTime >= diapersDTO.FromDate && x.DateTime <= diapersDTO.ToDate))
                .Where(x => x.Child.Parents.Contains(user))
                .ToListAsync(ct);

        return diapers.OrderBy(x => x.DateTime).ToList();
    }
}