using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DDDC.DAL
{
    public partial class DDDCModel1 : DbContext
    {
        public DDDCModel1()
            : base("name=DDDCModel1")
        {
        }

        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<AfterSales> AfterSales { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<OrderForm> OrderForm { get; set; }
        public virtual DbSet<ordernews> ordernews { get; set; }
        public virtual DbSet<orderT> orderT { get; set; }
        public virtual DbSet<payment> payment { get; set; }
        public virtual DbSet<ship_maintain> ship_maintain { get; set; }
        public virtual DbSet<ShipHandle> ShipHandle { get; set; }
        public virtual DbSet<ships> ships { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<orderT>()
                .Property(e => e.total_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ship_maintain>()
                .Property(e => e.description)
                .IsUnicode(false);
        }
    }
}
