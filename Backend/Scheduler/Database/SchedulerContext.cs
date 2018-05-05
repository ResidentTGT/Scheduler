namespace Scheduler.Database
{
    using Scheduler.Model;
    using Scheduler.Model.OrderReport;
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

            modelBuilder.Entity<Order>()
               .HasMany(o => o.OrderReports)
               .WithRequired(r => r.Order);


            modelBuilder.Entity<ProductionItem>()
                .HasMany(p => p.ProductionItemQuantums)
                .WithRequired(d => d.ProductionItem);


            modelBuilder.Entity<OrderQuantum>()
                .HasRequired(oq => oq.ProductionItem)
                .WithMany(pi => pi.OrderQuantums);


            modelBuilder.Entity<ProductionItemQuantum>()
                .HasRequired(piq => piq.Detail)
                .WithMany(pi => pi.ProductionItemQuantums);


            modelBuilder.Entity<Route>()
                .HasMany(r => r.Operations)
                .WithMany(o => o.Routes);

            modelBuilder.Entity<Workshop>()
                .HasMany(r => r.Equipments)
                .WithOptional(o => o.Workshop);

            modelBuilder.Entity<Conveyor>()
                .HasMany(r => r.Equipments)
                .WithOptional(o => o.Conveyor);

            modelBuilder.Entity<OrderReport>()
               .HasMany(o => o.OrderBlocks)
               .WithRequired(b => b.OrderReport);

            modelBuilder.Entity<OrderBlock>()
               .HasMany(o => o.GroupBlocks)
               .WithRequired(b => b.OrderBlock);

            modelBuilder.Entity<GroupBlock>()
             .HasMany(o => o.DetailsBatchBlocks)
             .WithRequired(b => b.GroupBlock);
            modelBuilder.Entity<GroupBlock>()
            .HasRequired(o => o.Workshop);

            modelBuilder.Entity<DetailsBatchBlock>()
           .HasRequired(o => o.Equipment);

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

        public virtual DbSet<Workshop> Workshops { get; set; }

        public virtual DbSet<Conveyor> Conveyors { get; set; }

        public virtual DbSet<OrderReport> OrderReports { get; set; }

        public virtual DbSet<DetailsBatchBlock> DetailsBatchBlocks { get; set; }

        public virtual DbSet<GroupBlock> GroupBlocks { get; set; }

        public virtual DbSet<OrderBlock> OrderBlocks { get; set; }

    }

}