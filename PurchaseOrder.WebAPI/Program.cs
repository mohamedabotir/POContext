using Application.Commands;
using Confluent.Kafka;
using Common.Events;
using Common.Utils;
using Domain.DomainEvents;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Infrastructure.Context;
using Infrastructure.Exceptions;
using Infrastructure.Mongo;
using Infrastructure.Producers;
using Infrastructure.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PurchaseOrder.WebAPI.Extensions;
using PurchaseOrder.WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextServices(builder)
    .AddMongoSerializers()
    .AddConfigurationService(builder)
    .AddUsecases()
    .AddRepositories()
    .AddDispatchers()
    .AddExternalServices()
    .AddGraphQlService()
    .AddElasticSearchService()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpContextAccessor();
builder.Host.UseSerilog();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground("/graphql-ui" , new PlaygroundOptions()
{
    GraphQLEndPoint = "/graphql"
});

app.UseMiddleware<CorrelationMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapPost("/orders", async ([FromBody]PurchaseOrdersCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    if (result.Results.Any(r=>r.IsFailure))
    {
      return  Results.BadRequest(result.Results.Select(e=>e.Message));
    }
    return Results.Created();
    })
    .WithName("add orders")
    .WithOpenApi();
app.MapPost("/order/{poNumber}", async (string poNumber,IMediator mediator) =>
    {
        var result = await mediator.Send(new PoApproveCommand(poNumber));
        if (result.IsFailure)
        {
            return  Results.BadRequest(result.Message);
        }
        return Results.Created();
    })
    .WithName("approve  order")
    .WithOpenApi();


app.Run();
