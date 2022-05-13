using BMSAPI.Database.Models;

namespace BMSAPI.Repositories;

public interface IGenericRepository<T> where T : class, IEntity {
    Task<List<T>?> GetAll(CancellationToken ct);
    Task<T?> Get(string id, CancellationToken ct);

    Task<T?> Create(T entity, CancellationToken ct);

    // Task<T> Update(T entity, CancellationToken ct);
    Task<bool> Delete(string id, CancellationToken ct);
    Task<bool> SaveAsync(CancellationToken ct);
    Task DisposeAsync(CancellationToken ct);
}