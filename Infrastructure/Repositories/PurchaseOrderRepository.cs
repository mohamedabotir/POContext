using System.Linq.Expressions;
using Domain.Entity;
using Domain.Repository;

namespace Application.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    public Task<PoEntity> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PoEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(PoEntity entity)
    {
        Console.WriteLine("Add PurchaseOrder");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PoEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PoEntity>> FindAsync(Expression<Func<PoEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task MarkPoAsShippedAsync(Guid poId)
    {
        Console.WriteLine("Mark PO as shipped:"+poId);
        return Task.CompletedTask;
    }
}