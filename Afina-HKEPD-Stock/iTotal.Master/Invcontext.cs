using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class InvContext : DbContext
    {
        public InvContext(string connString) :base(GetOptions(connString))
        {
        }
        public InvContext(DbContextOptions<InvContext> options)
            : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
        public virtual DbSet<sysMenu> sysMenu { get; set; }
        public virtual DbSet<Stock> StockMaster { get; set; }
        public virtual DbSet<StockTX> StockTX { get; set; }
    }
}
