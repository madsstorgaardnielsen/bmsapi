using System.Linq;
using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories; 

public class SleepRepository  : GenericRepository<Sleep, DatabaseContext> {
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public SleepRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }
    
    public async Task<List<Feeding>>
        GetAllSleeps(string username, GetAllSleepsDTO sleepsDTO, CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .SingleOrDefaultAsync(ct);

        var feedings = await
            _dbContext
                .Feedings
                .Include(x => x.Child)
                .Include(x => x.Child.Parents)
                .Where(x =>
                    (sleepsDTO.From == null || sleepsDTO.To == null) ||
                    (x.DateTime >= sleepsDTO.From && x.DateTime <= sleepsDTO.To))
                .Where(x => x.Child.Id == sleepsDTO.ChildId)
                .Where(x => x.Child.Parents.Contains(user))
                .ToListAsync(ct);

        return feedings.OrderBy(x => x.DateTime).ToList();
    }
    
}