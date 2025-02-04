namespace Infrastructure.Mongo;

public class PurchaseOrderConfig
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string CollectionName { get; init; }
}
