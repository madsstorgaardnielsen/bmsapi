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
    private readonly IMapper _mapper;

    public FeedingRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<Feeding>>
        GetAllFeedings(string username, GetAllFeedingDTO feedingDTO, CancellationToken ct) {
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
                    (feedingDTO.From == null || feedingDTO.To == null) ||
                    (x.DateTime >= feedingDTO.From && x.DateTime <= feedingDTO.To))
                .Where(x => x.Child.Id == feedingDTO.ChildId)
                .Where(x => x.Child.Parents.Contains(user))
                .ToListAsync(ct);

        return feedings.OrderBy(x => x.DateTime).ToList();
    }
}