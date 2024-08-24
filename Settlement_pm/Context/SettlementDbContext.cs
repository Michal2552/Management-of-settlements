using Microsoft.EntityFrameworkCore;
using Settlement_pm.Models;

namespace Settlement_pm.Context
{
    public class SettlementDbContext : DbContext
    {
        public SettlementDbContext(DbContextOptions<SettlementDbContext> contextOptions) : base(contextOptions) { }

        public DbSet<Settlement> Settlements { get; set; }
    }
}


