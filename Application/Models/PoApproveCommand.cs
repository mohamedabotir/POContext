using Common.Result;
using MediatR;

namespace Application.UseCases.PO.Models;

public record PoApproveCommand (string PurchaseOrderNumber): IRequest<Result>;
