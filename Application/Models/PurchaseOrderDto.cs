using Common.Result;
using Common.Utils;
using Common.ValueObject;
using MediatR;

namespace Application.UseCases.PO.Models;

public record PurchaseOrderDto(
    Guid RootGuid,
    UserDto Customer,
    UserDto Supplier,
    List<ItemLineDto> ItemLines,
    NumberGenerator NumberGenerator,string address);

public record ItemLineDto(Quantity Quantity, string Name, decimal Price, string Sku,Guid Guid);
public record UserDto(string Email, string PhoneNumber, string Name);
public record PurchaseOrdersDto(List<PurchaseOrderDto> PurchaseOrders):IRequest<Result>;
