using Common.Repository;
using Common.Result;

namespace Application.UseCases.PO;

public class PurchaseOrderApproveUseCase(IUnitOfWork unitOfWork,IPurchaseOrderRepository purchaseOrderRepository):IPurchaseOrderApproveUseCase
{
    public async Task<Result> Approve(string purchaseOrderId, CancellationToken cancellationToken)
    {
        using (unitOfWork)
        {
            var purchaseOrder =await purchaseOrderRepository.GetPoByPurchaseNumberAsync(purchaseOrderId);
            if(purchaseOrder.IsFailure)
                return Result.Fail(purchaseOrder.Message);
            var approvedOrder = purchaseOrder.Value.ApprovePurchase();
            if(approvedOrder.IsFailure)
                return Result.Fail(approvedOrder.Message);
            await purchaseOrderRepository.UpdatePoStageAsync(purchaseOrder.Value.Guid,purchaseOrder.Value.PurchaseOrderStage);
            await  unitOfWork.SaveChangesAsync(purchaseOrder.Value.DomainEvents, cancellationToken);
            
            return Result.Ok(approvedOrder);
        }
    }
}