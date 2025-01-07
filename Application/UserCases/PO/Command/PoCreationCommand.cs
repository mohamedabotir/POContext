using Application.UserCases.PO.Models;
using MediatR;

namespace Application.UserCases.PO.Command;

public class PoCreationCommand(IPurchaseOrderUseCase purchaseOrderUseCase) : IRequest<PurchaseOrderDto>
{
    public async Task Handle(PurchaseOrderDto purchaseOrder)
    {
        await purchaseOrderUseCase.CreatePurchaseOrder(purchaseOrder);
    }
}