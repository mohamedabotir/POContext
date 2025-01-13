using Application.UseCases.PO.Models;
using Domain.Entity;
using Domain.Repository;
using Domain.Result;
using Domain.ValueObject;

namespace Application.UseCases.PO;

public class PurchaseOrderUseCase(IUnitOfWork unitOfWork,IPurchaseOrderRepository purchaseOrderRepository)
    : IPurchaseOrderUseCase
{
    public async Task<Result> CreatePurchaseOrder(List<PurchaseOrderDto> purchaseOrdersDto)
    {
        
        using (unitOfWork)
        {
            var results  = Result.Ok();
            foreach (var purchaseOrderDto in purchaseOrdersDto)
            {
                if (purchaseOrderRepository.IsPoExists(purchaseOrderDto.RootGuid))
                {
                  results.AddResult(Result.Fail("PO already exists"));
                    continue;   
                }
                var money = Money.CreateInstance(purchaseOrderDto.TotalAmount);
                var customerEmail = Email.CreateInstance(purchaseOrderDto.Customer.Email);
                var supplierEmail = Email.CreateInstance(purchaseOrderDto.Supplier.Email);
                var validations = Result.Combine(money,customerEmail,supplierEmail);

                if (validations.IsFailure)
                {
                    results.AddResult(validations);
                    continue;
                }
            
                var customerUser = User.CreateInstance(customerEmail.Value, purchaseOrderDto.Customer.Name
                    , purchaseOrderDto.Customer.PhoneNumber);
                var supplierUser = User.CreateInstance(supplierEmail.Value, purchaseOrderDto.Supplier.Name
                    , purchaseOrderDto.Supplier.PhoneNumber);
                var poNumber = PoNumber.CreateInstance(purchaseOrderDto.NumberGenerator);
                validations = Result.Combine(money,customerUser,supplierUser,poNumber);
                if (validations.IsFailure)
                {
                    results.AddResult(validations);
                    continue;    
                }
                var purchaseOrder = new PoEntity(purchaseOrderDto.RootGuid,
                    customerUser.Value, supplierUser.Value,poNumber.Value);
                var lineItems = purchaseOrderDto.ItemLines.Select(ItemLineDtoToLineItemEntity).ToList();
                purchaseOrder.AddLineItems(lineItems);
                await purchaseOrderRepository.AddAsync(purchaseOrder)!;
                await unitOfWork.SaveChangesAsync(purchaseOrder.DomainEvents);
                results.AddResult(Result.Ok($"Po created Successfully For :{purchaseOrderDto.RootGuid}"));
            }
            return results;
        }
    }

    private LineItem ItemLineDtoToLineItemEntity(ItemLineDto itemLineDto) 
        => new LineItem(itemLineDto.Quantity,new Item(itemLineDto.Name,itemLineDto.Price,itemLineDto.Sku),itemLineDto.Guid,0);
}