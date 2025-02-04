using Common.Result;

namespace Application.UseCases;

public interface IPurchaseOrderApproveUseCase
{
    Task<Result> Approve(string purchaseOrderId,CancellationToken cancellationToken);
}