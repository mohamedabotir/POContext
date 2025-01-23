using Common.Events;
using Common.Entity;
using Common.Handlers;
using Common.Repository;
using Common.DomainEvents;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly PurchaseOrderDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IHttpContextAccessor _httpContextAccessor;
   

    public UnitOfWork(PurchaseOrderDbContext dbContext,IServiceProvider serviceProvider,IEventDispatcher eventDispatcher
        ,IHttpContextAccessor httpContextAccessor)
    {
        _context = dbContext;
        _serviceProvider = serviceProvider;
        _eventDispatcher = eventDispatcher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> SaveChangesAsync(IEnumerable<DomainEventBase> events,CancellationToken cancellationToken = default)
    {
     
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var correlationId = string.Empty;

        try
        {
            correlationId = _httpContextAccessor.HttpContext!.GetCorrelationId();
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (events.Any())
            {
                await _eventDispatcher.DispatchDomainEventsAsync(events);
            }

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            var exceptionMessage = string.Format("An error occured while Process Request please use this correlation id and contact help desk team {0}", correlationId);
            throw new InvalidOperationException("An error occurred during SaveChangesAsync", ex);
        }
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