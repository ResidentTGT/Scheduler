namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCostProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "Cost", c => c.Int(nullable: false));
            AddColumn("dbo.Equipments", "UsingTimeResource", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Equipments", "LoadFactor", c => c.Double(nullable: false));
            AddColumn("dbo.Equipments", "MaintenanceCost", c => c.Int(nullable: false));
            AddColumn("dbo.Operations", "RiggingCost", c => c.Int(nullable: false));
            AddColumn("dbo.Operations", "RiggingStorageCost", c => c.Int(nullable: false));
            AddColumn("dbo.ProductionItems", "OneItemIncome", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "Cost", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "Cost");
            DropColumn("dbo.ProductionItems", "OneItemIncome");
            DropColumn("dbo.Operations", "RiggingStorageCost");
            DropColumn("dbo.Operations", "RiggingCost");
            DropColumn("dbo.Equipments", "MaintenanceCost");
            DropColumn("dbo.Equipments", "LoadFactor");
            DropColumn("dbo.Equipments", "UsingTimeResource");
            DropColumn("dbo.Equipments", "Cost");
        }
    }
}
