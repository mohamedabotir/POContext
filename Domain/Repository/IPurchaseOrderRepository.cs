using Common.Constants;
using Common.Entity;
using Common.Result;

namespace Common.Repository;

public interface IPurchaseOrderRepository : IRepository<PoEntity>
{
    Task MarkPoAsShippedAsync(Guid poId);
    bool IsPoExists(Guid poId);
    Task<Result<PoEntity>> GetPoByPurchaseNumberAsync(string poId);
    Task UpdatePoStageAsync(Guid poId,PurchaseOrderStage stage);
};
