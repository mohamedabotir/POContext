using Application.UseCases.PO.Models;
using Common.Result;

namespace Application.UseCases.PO;

public interface IPurchaseOrderUseCase
{
    Task<Result> CreatePurchaseOrder(List<PurchaseOrderDto> purchaseOrdersDto);
}