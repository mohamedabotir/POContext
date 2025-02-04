using Application.Commands;
using Common.Constants;
using Common.Entity;
using Common.Repository;
using Common.Result;
using Common.Utils;
using Common.ValueObject;
using Domain.Entity;

namespace Application.UseCases;

public class PurchaseOrderCreationCreationUseCase(IUnitOfWork unitOfWork,IPurchaseOrderRepository purchaseOrderRepository)
    : IPurchaseOrderCreationUseCase
{
    public async Task<Result> CreatePurchaseOrder(List<PurchaseOrderCommand> purchaseOrdersDto)
    {
        
        using (unitOfWork)
        {
            var results  = Result.Ok();
            foreach (var purchaseOrderDto in purchaseOrdersDto)
            {
                if (purchaseOrderRepository.IsPoExists(purchaseOrderDto.RootGuid))
                {
                  results.AddResult(Result.Fail($"PO({purchaseOrderDto.RootGuid}) already exists"));
                    continue;   
                }
                var customerEmail = Email.CreateInstance(purchaseOrderDto.Customer.Email);
                var supplierEmail = Email.CreateInstance(purchaseOrderDto.Supplier.Email);
                var userLocation = Address.CreateInstance(purchaseOrderDto.address);

                var validations = Result.Combine(customerEmail,supplierEmail,userLocation);

                if (validations.IsFailure)
                {
                    results.AddResult(validations);
                    continue;
                }
            
                var customerUser = User.CreateInstance(customerEmail.Value, purchaseOrderDto.Customer.Name
                    , purchaseOrderDto.Customer.PhoneNumber,userLocation.Value);
                var supplierUser = User.CreateInstance(supplierEmail.Value, purchaseOrderDto.Supplier.Name
                    , purchaseOrderDto.Supplier.PhoneNumber,userLocation.Value);
                var poNumber = PoNumber.CreateInstance(purchaseOrderDto.NumberGenerator);
                validations = Result.Combine(customerUser,supplierUser,poNumber);
                if (validations.IsFailure)
                {
                    results.AddResult(validations);
                    continue;    
                }
                var purchaseOrder = new PoEntity(purchaseOrderDto.RootGuid,
                    customerUser.Value, supplierUser.Value,poNumber.Value,PurchaseOrderStage.Created);
                var lineItems = purchaseOrderDto.ItemLines.Select(ItemLineDtoToLineItemEntity).ToList();
                var addLineResult = purchaseOrder.AddLineItems(lineItems);
                if(addLineResult.IsFailure)
                {
                    results.AddResult(addLineResult);
                    continue;
                }
                
                await purchaseOrderRepository.AddAsync(purchaseOrder);
                await unitOfWork.SaveChangesAsync(purchaseOrder.DomainEvents);
                results.AddResult(Result.Ok($"Po created Successfully For :{purchaseOrderDto.RootGuid}"));
            }
            return results;
        }
    }

    private LineItem ItemLineDtoToLineItemEntity(ItemLineDto itemLineDto) 
        => new LineItem(itemLineDto.Quantity,new Item(itemLineDto.Name,itemLineDto.Price,itemLineDto.Sku),itemLineDto.Guid,0,0);
}