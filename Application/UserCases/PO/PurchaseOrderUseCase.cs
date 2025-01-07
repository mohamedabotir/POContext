using Application.UserCases.PO.Models;
using Domain.Entity;
using Domain.Handlers;
using Domain.Repository;
using Domain.ValueObject;

namespace Application.UserCases.PO;

public class PurchaseOrderUseCase(IUnitOfWork unitOfWork)
    : IPurchaseOrderUseCase
{
    public async Task CreatePurchaseOrder(PurchaseOrderDto purchaseOrderDto)
    {
        
        using (unitOfWork)
        {
            var purchaseOrder = new PoEntity(purchaseOrderDto.TotalAmount, purchaseOrderDto.RootGuid,
                purchaseOrderDto.Customer, purchaseOrderDto.Supplier);
            var lineItems = purchaseOrderDto.ItemLines.Select(ItemLineDtoToLineItemEntity).ToList();
            purchaseOrder.AddLineItems(lineItems);
           await unitOfWork.GetRepository<PoEntity>()
                .AddAsync(purchaseOrder);
            await unitOfWork.SaveChangesAsync();
        }
    }

    private LineItem ItemLineDtoToLineItemEntity(ItemLineDto itemLineDto) 
        => new LineItem(itemLineDto.Quantity,new Item(itemLineDto.Name,itemLineDto.Price,itemLineDto.Sku));
}