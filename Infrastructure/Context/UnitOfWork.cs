using Common.Events;
using Common.Entity;
using Common.Handlers;
using Common.Repository;
using Common.DomainEvents;
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