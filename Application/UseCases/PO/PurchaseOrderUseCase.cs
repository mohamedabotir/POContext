using Application.UseCases.PO.Models;
using Domain.Entity;
using Domain.Handlers;
using Domain.Repository;
using Domain.Result;
using Domain.ValueObject;

namespace Application.UseCases.PO;

public class PurchaseOrderUseCase(IUnitOfWork unitOfWork)
    : IPurchaseOrderUseCase
{
    public async Task<Result> CreatePurchaseOrder(List<PurchaseOrderDto> purchaseOrdersDto)
    {
        
        using (unitOfWork)
        {
            foreach (var purchaseOrderDto in purchaseOrdersDto)
            {
            var money = Money.CreateInstance(purchaseOrderDto.TotalAmount);
            var customerEmail = Email.CreateInstance(purchaseOrderDto.Customer.Email);
            var supplierEmail = Email.CreateInstance(purchaseOrderDto.Supplier.Email);
            
            var validations = Result.Combine(money,customerEmail,supplierEmail);
          
            if (validations.IsFailure)
                return Result.Fail(validations.Error);
            
            var customerUser = User.CreateInstance(customerEmail.Value, purchaseOrderDto.Customer.Name
                , purchaseOrderDto.Customer.PhoneNumber);
            var supplierUser = User.CreateInstance(supplierEmail.Value, purchaseOrderDto.Supplier.Name
                , purchaseOrderDto.Supplier.PhoneNumber);
            var poNumber = PoNumber.CreateInstance(purchaseOrderDto.NumberGenerator);
            validations = Result.Combine(money,customerUser,supplierUser,poNumber);
            if (validations.IsFailure)
                return Result.Fail(validations.Error);
            
            var purchaseOrder = new PoEntity(money.Value, purchaseOrderDto.RootGuid,
                customerUser.Value, supplierUser.Value,poNumber.Value);
            var lineItems = purchaseOrderDto.ItemLines.Select(ItemLineDtoToLineItemEntity).ToList();
            purchaseOrder.AddLineItems(lineItems);
           await unitOfWork.GetRepository<PoEntity>()
                .AddAsync(purchaseOrder);
            await unitOfWork.SaveChangesAsync(purchaseOrder.DomainEvents);
            }
            return Result.Ok();
        }
    }

    private LineItem ItemLineDtoToLineItemEntity(ItemLineDto itemLineDto) 
        => new LineItem(itemLineDto.Quantity,new Item(itemLineDto.Name,itemLineDto.Price,itemLineDto.Sku),itemLineDto.Guid);
}