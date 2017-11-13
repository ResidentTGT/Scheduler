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
            modelBuilder.Entity<Operation>()
               .HasRequired(e => e.Equipment)
               .WithMany(o => o.Operations);

            modelBuilder.Entity<Operation>()
               .HasRequired(e => e.Detail)
               .WithMany(o => o.Operations);


            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderQuantums)
                .WithRequired(poi => poi.Order);


            modelBuilder.Entity<ProductionItem>()
                .HasMany(p => p.ProductionItemQuantums)
                .WithRequired(d => d.ProductionItem);


            modelBuilder.Entity<OrderQuantum>()
                .HasRequired(oq => oq.ProductionItem)
                .WithMany(pi => pi.OrderQuantums);


            modelBuilder.Entity<Detail>()
                .HasOptional(d => d.Route)
                .WithOptionalDependent(r => r.Detail);

            modelBuilder.Entity<ProductionItemQuantum>()
                .HasRequired(piq => piq.Detail)
                .WithMany(pi => pi.ProductionItemQuantums);


            modelBuilder.Entity<Route>()
                .HasMany(r => r.Operations)
                .WithMany(o => o.Routes);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Equipment> Equipments { get; set; }

        public virtual DbSet<Operation> Operations { get; set; }

        public virtual DbSet<Detail> Details { get; set; }

        public virtual DbSet<ProductionItemQuantum> ProductionItemQuantums { get; set; }

        public virtual DbSet<ProductionItem> ProductionItems { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderQuantum> OrderQuantums { get; set; }

        public virtual DbSet<Route> Routes { get; set; }

    }

}