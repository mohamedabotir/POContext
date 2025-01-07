using Domain.ValueObject;

namespace Application.UserCases.PO.Models;

public record PurchaseOrderDto(decimal TotalAmount, Guid RootGuid, User Customer, User Supplier,List<ItemLineDto> ItemLines);

public record ItemLineDto(Quantity Quantity, string Name, decimal Price, string Sku);
