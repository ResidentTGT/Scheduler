namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimesToConveyor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conveyors", "TransportTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Conveyors", "ReadjustingTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conveyors", "ReadjustingTime");
            DropColumn("dbo.Conveyors", "TransportTime");
        }
    }
}
