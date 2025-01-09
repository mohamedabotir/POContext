using Application.UserCases.PO.Models;
using MediatR;

namespace Application.UserCases.PO.Command;

public class PoCreationCommandHandler(IPurchaseOrderUseCase purchaseOrderUseCase) : IRequestHandler<PurchaseOrderDto>
{
    public async Task<Unit> Handle(PurchaseOrderDto purchaseOrder, CancellationToken cancellationToken)
    {
        await purchaseOrderUseCase.CreatePurchaseOrder(purchaseOrder);
        return Unit.Value;
    }
}