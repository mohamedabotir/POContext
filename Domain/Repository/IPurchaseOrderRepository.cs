using Common.Repository;
using Domain.Entity;

namespace Domain.Repository;

public interface IPurchaseOrderRepository : IRepository<PoEntity>
{
    Task MarkPoAsShippedAsync(Guid poId);
    bool IsPoExists(Guid poId);

};
