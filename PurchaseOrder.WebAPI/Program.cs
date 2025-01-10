using System.Reflection;
using Application.UseCases.PO;
using Application.UseCases.PO.Command;
using Application.UseCases.PO.Models;
using Confluent.Kafka;
using Domain.DomainEvents;
using Domain.Entity;
using Domain.Handlers;
using Domain.Mongo;
using Domain.Mongo.Producers;
using Domain.Repository;
using Application.Context;
using Application.EventHandlers;
using Application.Extensions;
using Application.Mongo;
using Application.Repositories;
using Domain.Result;
using Infrastructure.Consumer;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using EventHandler = Infrastructure.Consumer.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonClassMap.RegisterClassMap<DomainEventBase>();
BsonClassMap.RegisterClassMap<PoCreatedEventBase>();

builder.Services.Configure<Topic>(builder.Configuration.GetSection("Topic"));
builder.Services.Configure<PurchaseOrderConfig>(builder.Configuration.GetSection("MongoConfig"));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("ProducerConfig"));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("ConsumerConfig"));
builder.Services.AddSingleton<IProducer, Producer>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEventStore, PurchaseOrderEventStore>();
builder.Services.AddTransient<IDomainEventHandler<PoCreatedEventBase>, PurchaseOrderCreationHandler>();
builder.Services.AddTransient<IPurchaseOrderUseCase, PurchaseOrderUseCase>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddSingleton<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddSingleton<IRepository<PoEntity>, PurchaseOrderRepository>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IRequestHandler<PurchaseOrderDto, Result>, PoCreationCommandHandler>();
builder.Services.AddTransient<IEventDispatcher, EventDispatcher>();

// consumers
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.AddScoped<IEventConsumer<EventConsumer>, EventConsumer>();
builder.Services.AddHostedService<ConsumerHostingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/weatherforecast", async ([FromBody]PurchaseOrderDto command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    if (result.IsFailure)
    {
      return  Results.BadRequest(result.Error);
    }
    return Results.Created();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}