using Application.UseCases.PO.Models;

namespace Application.UseCases.PO;

public interface IPurchaseOrderUseCase
{
    Task CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto);
}