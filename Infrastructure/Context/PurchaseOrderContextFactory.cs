using Application.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PurchaseOrderContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    public PurchaseOrderContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }

    public PurchaseOrderDbContext CreateDataBaseContext()
    {

        DbContextOptionsBuilder<PurchaseOrderDbContext> optionsBuilder = new();
        _configureDbContext(optionsBuilder);


        return new PurchaseOrderDbContext(optionsBuilder.Options);

    }
}