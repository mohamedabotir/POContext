using Domain.Entity;
using Domain.Handlers;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public class UnitOfWork(DbContext _context, IServiceProvider _serviceProvider,IEventDispatcher eventDispatcher): IUnitOfWork
{
  
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var result = await _context.SaveChangesAsync(cancellationToken);

        await eventDispatcher.DispatchDomainEventsAsync(domainEvents);

        return result;
        
    }

    public IRepository<T>? GetRepository<T>() where T : AggregateRoot
    {
        return _serviceProvider.GetService<Domain.Repository.IRepository<T>>();
    }
    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

}