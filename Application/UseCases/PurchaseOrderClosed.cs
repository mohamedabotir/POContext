using Common.Events;
using Common.Repository;
using Common.Result;

namespace Application.UseCases;

public class PurchaseOrderClosed(IPurchaseOrderRepository purchaseOrderRepository):IPurchaseOrderClosed
{
    public async Task<Result> ClosePurchaseOrder(OrderClosed @event)
    {
        var purchaseOrder =await purchaseOrderRepository.GetPoByPurchaseNumberWithFactoryAsync(@event.PoNumber);
        if(purchaseOrder.IsFailure)
            return Result.Fail(purchaseOrder.Message);
        var purchaseOrderClosed = purchaseOrder.Value.ClosePurchaseOrder();
      await  purchaseOrderRepository.UpdatePoStageWithFactoryAsync(purchaseOrder.Value.Guid, purchaseOrder.Value.PurchaseOrderStage);
      return Result.Ok();
    }
}