using System.Linq.Expressions;
using Application.Context;
using Application.Context.Pocos;
using Domain.Entity;
using Domain.Repository;

namespace Infrastructure.Repository;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private PurchaseOrderDbContext _context;
    public PurchaseOrderRepository(PurchaseOrderDbContext dbContext)
    {
        _context = dbContext;
    }
    public Task<PoEntity> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PoEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(PoEntity entity)
    {
        _context.Set<PurchaseOrder>();
        var purchaseOrder = new PurchaseOrder();
        purchaseOrder.MapPoEntityToPurchaseOrder(entity);
       await _context.AddAsync(purchaseOrder);
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