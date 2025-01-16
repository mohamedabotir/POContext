using Common.Result;

namespace Application.UseCases.PO;

public interface IPurchaseOrderApproveUseCase
{
    Task<Result> Approve(string purchaseOrderId,CancellationToken cancellationToken);
}