using Application.UseCases.PO.Models;
using Domain.Result;
using MediatR;

namespace Application.UseCases.PO.Command;

public class PoCreationCommandHandler(IPurchaseOrderUseCase purchaseOrderUseCase) : IRequestHandler<PurchaseOrdersDto,Result>
{
    public async Task<Result> Handle(PurchaseOrdersDto purchaseOrders, CancellationToken cancellationToken)
    {
        var result = await purchaseOrderUseCase.CreatePurchaseOrder(purchaseOrders.PurchaseOrders);
        return result;
    }
}