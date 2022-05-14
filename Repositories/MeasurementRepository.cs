using System.Linq;
using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public class MeasurementRepository : GenericRepository<Measurement, DatabaseContext> {
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public MeasurementRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<Measurement>> GetAllMeasurementsByUsername(string username, string childId,
        CancellationToken ct) {
        var user = await
            _dbContext
                .Users
                .Include(x => x.Children)
                .Where(x => x.UserName == username)
                .SingleOrDefaultAsync(ct);
        
        var measurements = await
            _dbContext
                .Measurements
                .Include(x => x.Child.Parents)
                .Where(x => x.ChildId == childId)
                .Where(x => x.Child.Parents.Contains(user)).ToListAsync(ct);

        return measurements.OrderBy(x => x.Date).ToList();
    }
}