namespace Scheduler.Database
{
    using Scheduler.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SchedulerContext : DbContext
    {

        public SchedulerContext()
            : base("name=SchedulerDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Operations)
                .WithRequired(o => o.Equipment);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ProductionItems)
                .WithMany(pi => pi.Orders);

            modelBuilder.Entity<ProductionItem>()
                .HasMany(p => p.Details)
                .WithMany(d => d.ProductionItems);

            modelBuilder.Entity<Detail>()
                .HasOptional(d => d.Route)
                .WithOptionalDependent(r => r.Detail);

            modelBuilder.Entity<Route>()
                .HasMany(r => r.Operations)
                .WithMany(o => o.Routes);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Equipment> Equipments { get; set; }

        public virtual DbSet<Operation> Operations { get; set; }

        public virtual DbSet<Detail> Details { get; set; }

        public virtual DbSet<ProductionItem> ProductionItems { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Route> Routes { get; set; }

    }

}