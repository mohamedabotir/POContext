using Application.Context.Pocos;
using Common.Entity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class PocoRepository
    {
        private readonly PurchaseOrderDbContext _context;

        public PocoRepository(PurchaseOrderDbContext dbContext)
        {
            _context = dbContext;
            _context.Set<PurchaseOrder>();
        }
        public async Task<IEnumerable<PurchaseOrder>> GetTopPurchaseOrdersAsync(int count)
        {
            return  await _context.PurchaseOrder
                .Include(e => e.LineItems)
                .OrderByDescending(e => e.CreatedOn)
                .Take(count)
                .ToListAsync();
        }
    }
}
