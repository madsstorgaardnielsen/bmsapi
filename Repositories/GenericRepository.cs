using BMSAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BMSAPI.Repositories;

public abstract class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext {
    private readonly TContext _context;

    protected GenericRepository(TContext context) {
        _context = context;
    }

    public async Task<TEntity?> Create(TEntity entity, CancellationToken ct) {
        _context.Set<TEntity>().Add(entity);
        await SaveAsync(ct);
        return entity;
    }

    public async Task<bool> Delete(string id, CancellationToken ct) {
        var entity = await _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id, ct);
        if (entity == null) return false;
        _context.Set<TEntity>().Remove(entity);
        await SaveAsync(ct);
        return true;
    }

    public async Task<bool> SaveAsync(CancellationToken ct) {
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task DisposeAsync(CancellationToken ct) {
        await _context.DisposeAsync();
    }

    public async Task<TEntity?> Get(string id, CancellationToken ct) {
        return await _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<TEntity>?> GetAll(CancellationToken ct) {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync(ct);
    }

    // public async Task<TEntity> Update(TEntity entity, CancellationToken ct) {
    //     _context.Set<TEntity>().Update(entity);
    //     await _context.SaveChangesAsync(ct);
    //     return entity;
    // }
}