using Application.UseCases.PO.Models;
using Domain.Result;
using MediatR;

namespace Application.UseCases.PO.Command;

public class PoCreationCommandHandler(IPurchaseOrderUseCase purchaseOrderUseCase) : IRequestHandler<PurchaseOrderDto,Result>
{
    public async Task<Result> Handle(PurchaseOrderDto purchaseOrder, CancellationToken cancellationToken)
    {
        var result = await purchaseOrderUseCase.CreatePurchaseOrder(purchaseOrder);
        return result;
    }
}