using Domain.DomainEvents;
using Domain.Entity;
using Domain.Handlers;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context1;
    private readonly IServiceProvider _serviceProvider1;
    private readonly IEventDispatcher _eventDispatcher;

   

    public UnitOfWork(IServiceProvider serviceProvider,IEventDispatcher eventDispatcher)
    {
        //_context1 = context;
        _serviceProvider1 = serviceProvider;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<int> SaveChangesAsync(IEnumerable<DomainEventBase> events,CancellationToken cancellationToken = default)
    {

        var result = 1;//await _context1.SaveChangesAsync(cancellationToken);

        await _eventDispatcher.DispatchDomainEventsAsync(events);

        return result;
        
    }

    public IRepository<T>? GetRepository<T>() where T : AggregateRoot
    {
        return _serviceProvider1.GetService<Domain.Repository.IRepository<T>>();
    }
    public void Dispose()
    {
        _context1?.Dispose();
        GC.SuppressFinalize(this);
    }

}