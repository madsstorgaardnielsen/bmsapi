using System.Linq;
using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public class MeasurementRepository : GenericRepository<Measurement, DatabaseContext> {
    private readonly DatabaseContext _dbContext;

    public MeasurementRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
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