using Common.Domains;
using Common.Entity;
using Common.Handlers;
using Common.Repository;
using Domain.DomainEvents;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly PurchaseOrderDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventDispatcher _eventDispatcher;

   

    public UnitOfWork(PurchaseOrderDbContext dbContext,IServiceProvider serviceProvider,IEventDispatcher eventDispatcher)
    {
        _context = dbContext;
        _serviceProvider = serviceProvider;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<int> SaveChangesAsync(IEnumerable<DomainEventBase> events,CancellationToken cancellationToken = default)
    {
     /*
      * 
        var domainEvents = _context1.ChangeTracker
            .Entries<AggregateRoot>() 
            .SelectMany(e => e.Entity.DomainEvents) 
            .Where(domainEvent => domainEvent != null)
            .ToList();

        foreach (var entry in _context1.ChangeTracker.Entries<AggregateRoot>())
        {
            entry.Entity.ClearEvents();
        }
      */

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (events.Any())
        {
            await _eventDispatcher.DispatchDomainEventsAsync(events);
        }

        return result;
    }


    public IRepository<T>? GetRepository<T>() where T : AggregateRoot
    {
        return _serviceProvider.GetService<IRepository<T>>();
    }
    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

}