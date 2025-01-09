using Domain.DomainEvents;
using Domain.Entity;

namespace Domain.Repository;

public interface IUnitOfWork: IDisposable
{
    public  Task<int> SaveChangesAsync(IEnumerable<DomainEventBase> events,CancellationToken cancellationToken = default);
    public IRepository<T>? GetRepository<T>() where T : AggregateRoot;
}