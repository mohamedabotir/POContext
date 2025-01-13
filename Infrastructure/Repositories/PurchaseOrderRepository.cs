using System.Linq.Expressions;
using Application.Context;
using Application.Context.Pocos;
using Common.Entity;
using Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private PurchaseOrderDbContext _context;
    public PurchaseOrderRepository(PurchaseOrderDbContext dbContext)
    {
        _context = dbContext;
        _context.Set<PurchaseOrder>();
    }
    public Task<PoEntity> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PoEntity>> GetAllAsync()
    {
        var purchaseOrder = _context.PurchaseOrder
            .Include(e=>e.LineItems)
            .Select(e => e)
            .ToList();
        var poEntities = purchaseOrder.Select(e => e.GetPoEntity()).ToList();
         return Task.FromResult<IEnumerable<PoEntity>>( poEntities.Select(e => e.Value).ToList());

    }

    public async Task AddAsync(PoEntity entity)
    {
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
    public bool IsPoExists(Guid poId) =>_context.PurchaseOrder.Any(e=>e.Guid == poId);
    
}