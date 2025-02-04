using Application.Commands;
using Common.Result;

namespace Application.UseCases;

public interface IPurchaseOrderCreationUseCase
{
    Task<Result> CreatePurchaseOrder(List<PurchaseOrderCommand> purchaseOrdersDto);
}