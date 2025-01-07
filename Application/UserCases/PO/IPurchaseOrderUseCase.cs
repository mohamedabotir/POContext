using Application.UserCases.PO.Models;

namespace Application.UserCases.PO;

public interface IPurchaseOrderUseCase
{
    Task CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto);
}