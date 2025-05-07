using System.Reflection;
using Application.Commands;
using Application.Handlers;
using Application.UseCases;
using Application.UseCases.PO.Command;
using Common.Entity;
using Common.Events;
using Common.Handlers;
using Common.Repository;
using Common.Result;
using Common.Utils;
using Common.ValueObject;
using Confluent.Kafka;
using Domain.DomainEvents;
using Domain.Entity;
using GraphQL;
using GraphQL.Types;
using Infrastructure.Consumer;
using Infrastructure.Context;
using Infrastructure.Exceptions;
using Infrastructure.Handlers;
using Infrastructure.Mongo;
using Infrastructure.Producers;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PurchaseOrder.WebAPI.GraphQl;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using EventHandler = Infrastructure.Handlers.EventHandler;

namespace PurchaseOrder.WebAPI.Extensions;

public static class ConfigurationServices
{
    public static IServiceCollection AddElasticSearchService(this IServiceCollection services)
    {
        var elkConfiguration = services.BuildServiceProvider().GetService(typeof(IOptions<ElkLog>)) as IOptions<ElkLog>;
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elkConfiguration!.Value.ConnectionString))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logs-{0:yyyy.MM.dd}"
            })
            .CreateLogger();


        return services;
    }
    public static IServiceCollection AddGraphQlService(this IServiceCollection services)
    {
        services.AddScoped<PurchaseOrderType>();
        services.AddScoped<LineItemsType>();
        services.AddScoped<PurchaseOrderQuery>();
        services.AddScoped<ActivationStatusEnumGraphType>();
        services.AddScoped<ISchema, PurchaseSchema>();
        services.AddGraphQL(b => b
            .AddAutoSchema<PurchaseOrderQuery>()  
            .AddSystemTextJson());
        return services;
    }
    public static IServiceCollection AddDispatchers(this IServiceCollection services)
    {
        services.AddTransient<IEventHandler, EventHandler>();
        services.AddTransient<IEventDispatcherWithFactory, EventDispatcherWithFactory>();
        services.AddTransient<IEventDispatcher, EventDispatcher>();
        services.AddTransient<IEventSourcing<PoEntity>, EventSourcing>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<IRequestHandler<PurchaseOrdersCommand, Result>, PoCreationCommandHandler>();
        services.AddTransient<IEventHandler<OrderClosed>, PurchaseOrderClosedHandler>();

        services.AddTransient<IRequestHandler<PoApproveCommand, Result>, PoApproveCommandHandler>();
        return services;
    }
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddTransient<IEventConsumer<EventConsumer>, EventConsumer>();
        services.AddTransient<IProducer, Producer>();
        services.AddHostedService<ConsumerHostingService>();
        //services.AddHostedService<OutboxProcessor>();
        return services;
    }
    public static IServiceCollection AddUsecases(this IServiceCollection services)
    {
        services.AddTransient<IPurchaseOrderCreationUseCase, PurchaseOrderCreationCreationUseCase>();
        services.AddTransient<IPurchaseOrderApproveUseCase, PurchaseOrderApproveUseCase>();
        services.AddTransient<IPurchaseOrderClosed, PurchaseOrderClosed>();
        services.AddSingleton<TopPoCacheService>();
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork<PoEntity>, UnitOfWork>();
        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddTransient<IRepository<PoEntity>, PurchaseOrderRepository>();
        services.AddTransient<PocoRepository>();
        return services;
    }
    public static IServiceCollection AddConfigurationService(this IServiceCollection services,WebApplicationBuilder builder)
    {
        services.Configure<Topic>(builder.Configuration.GetSection("Topic"));
        services.Configure<PurchaseOrderConfig>(builder.Configuration.GetSection("MongoConfig"));
        services.Configure<ProducerConfig>(builder.Configuration.GetSection("ProducerConfig"));
        services.Configure<ConsumerConfig>(builder.Configuration.GetSection("ConsumerConfig"));
        services.Configure<ElkLog>(builder.Configuration.GetSection(nameof(ElkLog)));
        return services;
    }
    public static IServiceCollection AddDbContextServices(this IServiceCollection services,WebApplicationBuilder builder)
    {
        void SqlConfiguration(DbContextOptionsBuilder e) => e.UseSqlServer(builder.Configuration.GetConnectionString("PurchaseOrderDB"));
        builder.Services.AddDbContext<PurchaseOrderDbContext>(SqlConfiguration);
        services.AddSingleton(new PurchaseOrderContextFactory(SqlConfiguration));
        services.AddSingleton<IServiceProviderFactory, ServiceProviderFactory>();
        return services;
    }
    public static IServiceCollection AddMongoSerializers(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonClassMap.RegisterClassMap<DomainEventBase>();
        BsonClassMap.RegisterClassMap<PoCreatedEventBase>();
        BsonClassMap.RegisterClassMap<OrderBeingShipped>();
        BsonClassMap.RegisterClassMap<EventModel>();
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));

        BsonClassMap.RegisterClassMap<Email>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<Address>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }); 
        BsonClassMap.RegisterClassMap<User>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
        BsonClassMap.RegisterClassMap<Quantity>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
        BsonClassMap.RegisterClassMap<Money>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
        BsonClassMap.RegisterClassMap<Item>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        }); 
        BsonClassMap.RegisterClassMap<LineItem>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        return services;
    }
}