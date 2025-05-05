using Common.Result;
using Common.Utils;
using Common.ValueObject;
using MediatR;

namespace Application.Commands;

public record PurchaseOrderCommand(
    Guid RootGuid,
    UserDto Customer,
    UserDto Supplier,
    List<ItemLineDto> ItemLines,
    NumberGenerator NumberGenerator,string address);



public record ItemLineDto(int QuantityValue, QuantityType QuantityType, string Name, decimal Price, string Sku,Guid Guid);
public record UserDto(string Email, string PhoneNumber, string Name);
public record PurchaseOrdersCommand(List<PurchaseOrderCommand> PurchaseOrders):IRequest<Result>;
