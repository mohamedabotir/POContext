using Common.Entity;
using Common.Events;
using Common.Repository;
using Common.Result;

namespace Application.UseCases;

public class PurchaseOrderApproveUseCase(IUnitOfWork<PoEntity> unitOfWork, IEventSourcing<PoEntity> @event,IPurchaseOrderRepository purchaseOrderRepository):IPurchaseOrderApproveUseCase
{
    public async Task<Result> Approve(string purchaseOrderId, CancellationToken cancellationToken)
    {
        using (unitOfWork)
        {
            var purchaseOrder = await @event.GetByIdAsync(purchaseOrderId);
            var approvedOrder = purchaseOrder.ApprovePurchase();
            if(approvedOrder.IsFailure)
                return Result.Fail(approvedOrder.Message);
            await purchaseOrderRepository.UpdatePoStageAsync(purchaseOrder.Guid,purchaseOrder.PurchaseOrderStage);
            await  unitOfWork.SaveChangesAsync(purchaseOrder, cancellationToken);
            return Result.Ok(approvedOrder);
        }
    }
}