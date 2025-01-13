using System.Reflection;
using Application.UseCases.PO;
using Application.UseCases.PO.Command;
using Application.UseCases.PO.Models;
using Confluent.Kafka;
using Common.DomainEvents;
using Common.Entity;
using Common.Mongo;
using Common.Mongo.Producers;
using Application.Context;
using Application.EventHandlers;
using Application.Extensions;
using Application.Mongo;
using Common.Domains;
using Common.Handlers;
using Common.Repository;
using Common.Result;
using Infrastructure.Repository;
using GraphQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Infrastructure.Consumer;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PurchaseOrder.WebAPI.GraphQl;
using EventHandler = Infrastructure.Consumer.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PurchaseOrderDbContext>(e=>e.
    UseSqlServer(builder.Configuration.GetConnectionString("PurchaseOrderDB")),ServiceLifetime.Scoped);
 
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonClassMap.RegisterClassMap<DomainEventBase>();
BsonClassMap.RegisterClassMap<PoCreatedEventBase>();

builder.Services.Configure<Topic>(builder.Configuration.GetSection("Topic"));
builder.Services.Configure<PurchaseOrderConfig>(builder.Configuration.GetSection("MongoConfig"));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("ProducerConfig"));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("ConsumerConfig"));
builder.Services.AddTransient<IProducer, Producer>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEventStore, PurchaseOrderEventStore>();
builder.Services.AddTransient<IEventHandler<PoCreatedEventBase>, PurchaseOrderCreationHandler>();
builder.Services.AddTransient<IPurchaseOrderUseCase, PurchaseOrderUseCase>();
builder.Services.AddTransient<IEventRepository, EventRepository>();
builder.Services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddTransient<IRepository<PoEntity>, PurchaseOrderRepository>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IRequestHandler<PurchaseOrdersDto, Result>, PoCreationCommandHandler>();
builder.Services.AddTransient<IEventDispatcher, EventDispatcher>();

// consumers
builder.Services.AddTransient<IEventHandler, EventHandler>();
builder.Services.AddTransient<IEventConsumer<EventConsumer>, EventConsumer>();
builder.Services.AddHostedService<ConsumerHostingService>();
//GraphQl
builder.Services.AddScoped<PurchaseOrderType>();
builder.Services.AddScoped<LineItemsType>();
builder.Services.AddScoped<PurchaseOrderQuery>();
builder.Services.AddScoped<ISchema, PurchaseSchema>();
builder.Services.AddGraphQL(b => b
    .AddAutoSchema<PurchaseOrderQuery>()  
    .AddSystemTextJson());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground("/graphql-ui" , new PlaygroundOptions()
{
    GraphQLEndPoint = "/graphql"
});
app.MapPost("/orders", async ([FromBody]PurchaseOrdersDto command, IMediator mediator) =>
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

app.Run();
