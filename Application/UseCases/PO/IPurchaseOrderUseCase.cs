using Application.UseCases.PO.Models;
using Domain.Result;

namespace Application.UseCases.PO;

public interface IPurchaseOrderUseCase
{
    Task<Result> CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto);
}