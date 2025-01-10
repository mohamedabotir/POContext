using Domain.Entity;

namespace Domain.Repository;

public interface IPurchaseOrderRepository : IRepository<PoEntity>
{
    Task MarkPoAsShippedAsync(Guid poId);
};
