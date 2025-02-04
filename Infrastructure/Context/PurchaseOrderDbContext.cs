using Application.Context.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PurchaseOrderDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<PurchaseOrder> PurchaseOrder { get; set; }

    public DbSet<LineItems> LineItems { get; set; }

   
}