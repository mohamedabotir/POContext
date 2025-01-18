using Common.Events;
using Common.Result;

namespace Application.UseCases;

public interface IPurchaseOrderClosed
{
    Task<Result> ClosePurchaseOrder(OrderClosed  @event);
}