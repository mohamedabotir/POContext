using Application.Commands;
using Common.Result;
using MediatR;

namespace Application.UseCases.PO.Command;

public class PoApproveCommandHandler(IPurchaseOrderApproveUseCase purchaseOrderApproveUseCase):IRequestHandler<PoApproveCommand,Result>
{
    public async Task<Result> Handle(PoApproveCommand request, CancellationToken cancellationToken)
    {
       return await purchaseOrderApproveUseCase.Approve(request.PurchaseOrderNumber, cancellationToken);
    }
}