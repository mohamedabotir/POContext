using Application.UseCases.PO.Models;
using Common.Result;
using MediatR;

namespace Application.UseCases.PO.Command;

public class PoCreationCommandHandler(IPurchaseOrderCreationUseCase purchaseOrderCreationUseCase) : IRequestHandler<PurchaseOrdersDto,Result>
{
    public async Task<Result> Handle(PurchaseOrdersDto purchaseOrders, CancellationToken cancellationToken)
    {
        var result = await purchaseOrderCreationUseCase.CreatePurchaseOrder(purchaseOrders.PurchaseOrders);
        return result;
    }
}