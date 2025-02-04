using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PurchaseOrderContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
{
    public PurchaseOrderDbContext CreateDataBaseContext()
    {

        DbContextOptionsBuilder<PurchaseOrderDbContext> optionsBuilder = new();
        configureDbContext(optionsBuilder);


        return new PurchaseOrderDbContext(optionsBuilder.Options);

    }
}