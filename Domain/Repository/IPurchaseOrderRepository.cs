using Common.Entity;
using Common.Repository;

namespace Common.Repository;

public interface IPurchaseOrderRepository : IRepository<PoEntity>
{
    Task MarkPoAsShippedAsync(Guid poId);
    bool IsPoExists(Guid poId);

};
