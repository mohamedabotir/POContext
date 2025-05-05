using Infrastructure.Producers;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Common.Events;
using Infrastructure.Mongo;
using Common.Repository;
using Domain.DomainEvents;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Confluent.Kafka;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<Topic>(context.Configuration.GetSection("Topic"));
        services.Configure<PurchaseOrderConfig>(context.Configuration.GetSection("MongoConfig"));
        services.Configure<ProducerConfig>(context.Configuration.GetSection("ProducerConfig"));

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonClassMap.RegisterClassMap<DomainEventBase>();
        BsonClassMap.RegisterClassMap<PoCreatedEventBase>();
        BsonClassMap.RegisterClassMap<PurchaseOrderApproved>();
        BsonClassMap.RegisterClassMap<OrderBeingShipped>();
        BsonClassMap.RegisterClassMap<EventModel>();
        services.AddSingleton<IEventRepository, EventRepository>();
        services.AddSingleton<IProducer, Producer>(); // assuming you have this
        services.AddTransient<OutboxProcessor>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var processor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();
await processor.RunAsync(CancellationToken.None);
