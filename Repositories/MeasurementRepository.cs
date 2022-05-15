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
                .Where(x => x.UserName == username)
                .Include(x => x.Children)
                .ThenInclude(x =>
                    x.Measurements
                        .Where(y => y.ChildId == childId))
                .SingleOrDefaultAsync(ct);

        var measurements = user?.Children.SingleOrDefault(x => x.Id == childId)?.Measurements.ToList();

        return measurements == null ? new List<Measurement>() : measurements.OrderBy(x => x.Date).ToList();
    }
}