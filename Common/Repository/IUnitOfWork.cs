using Common.Events;
using Common.Entity;

namespace Common.Repository;

public interface IUnitOfWork<TSource>: IDisposable
{
    public  Task<int> SaveChangesAsync(TSource events,CancellationToken cancellationToken = default);
    public IRepository<T>? GetRepository<T>() where T : AggregateRoot;
}