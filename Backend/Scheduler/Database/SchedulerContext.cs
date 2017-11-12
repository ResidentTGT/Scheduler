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

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Equipment> Equipments { get; set; }

        public virtual DbSet<Operation> Operations { get; set; }


    }

}