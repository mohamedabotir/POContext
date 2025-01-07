using Domain.Entity;

namespace Domain.Repository;

public interface IUnitOfWork: IDisposable
{
    public  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public IRepository<T>? GetRepository<T>() where T : AggregateRoot;
}