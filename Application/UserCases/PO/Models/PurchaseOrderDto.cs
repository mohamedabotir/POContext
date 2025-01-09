using Domain.ValueObject;
using MediatR;

namespace Application.UserCases.PO.Models;

public record PurchaseOrderDto(decimal TotalAmount, Guid RootGuid, User Customer, User Supplier,List<ItemLineDto> ItemLines):IRequest;

public record ItemLineDto(Quantity Quantity, string Name, decimal Price, string Sku);
