using AutoMapper;
using BMSAPI.Database;
using BMSAPI.Database.Models;

namespace BMSAPI.Repositories;

public class DiaperRepository : GenericRepository<Diaper, DatabaseContext> {
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public DiaperRepository(DatabaseContext context, IMapper mapper) : base(context) {
        _dbContext = context;
        _mapper = mapper;
    }
}