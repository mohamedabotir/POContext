using Application.Context.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Application.Context;

public class PurchaseOrderDbContext : DbContext
{
    public PurchaseOrderDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PurchaseOrder> PurchaseOrder { get; set; }

    public DbSet<LineItems> LineItems { get; set; }

   
}