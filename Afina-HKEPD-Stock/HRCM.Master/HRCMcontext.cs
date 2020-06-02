using HRCM.Master.Model;
using HRCM.Master.Model.System;
using iTotal.Base;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class HRCMContext : DbContext
    {
        public HRCMContext(string connString) :base(GetOptions(connString))
        {
        }
        public HRCMContext(DbContextOptions<HRCMContext> options)
            : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public virtual DbSet<Dept> Dept { get; set; }
        public virtual DbSet<Posi> Posi { get; set; }
        public virtual DbSet<Grad> Grad { get; set; }
        public virtual DbSet<Leav> Leav { get; set; }
        public virtual DbSet<CMPY> Cmpy { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<User> UserMaster { get; set; }
        public virtual DbSet<sysMenu> sysMenu { get; set; }
        public virtual DbSet<WorkTime> WorkTime { get; set; }
        public virtual DbSet<PosiProfile> PosiProfile { get; set; }
    }
}
