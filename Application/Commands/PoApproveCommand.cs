using Common.Result;
using MediatR;

namespace Application.Commands;

public record PoApproveCommand (string PurchaseOrderNumber): IRequest<Result>;
