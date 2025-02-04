using Application.UseCases;
using Application.UseCases.PO;
using Common.Result;
using MediatR;

namespace Application.Commands;

public class PoCreationCommandHandler(IPurchaseOrderCreationUseCase purchaseOrderCreationUseCase) : IRequestHandler<PurchaseOrdersCommand,Result>
{
    public async Task<Result> Handle(PurchaseOrdersCommand purchaseOrders, CancellationToken cancellationToken)
    {
        var result = await purchaseOrderCreationUseCase.CreatePurchaseOrder(purchaseOrders.PurchaseOrders);
        return result;
    }
}