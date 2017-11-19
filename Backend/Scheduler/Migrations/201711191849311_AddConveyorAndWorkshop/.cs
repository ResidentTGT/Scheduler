namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConveyorAndWorkshop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conveyors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workshops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Equipments", "WorkshopId", c => c.Int());
            AddColumn("dbo.Equipments", "ConveyorId", c => c.Int());
            CreateIndex("dbo.Equipments", "WorkshopId");
            CreateIndex("dbo.Equipments", "ConveyorId");
            AddForeignKey("dbo.Equipments", "WorkshopId", "dbo.Workshops", "Id");
            AddForeignKey("dbo.Equipments", "ConveyorId", "dbo.Conveyors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Equipments", "ConveyorId", "dbo.Conveyors");
            DropForeignKey("dbo.Equipments", "WorkshopId", "dbo.Workshops");
            DropIndex("dbo.Equipments", new[] { "ConveyorId" });
            DropIndex("dbo.Equipments", new[] { "WorkshopId" });
            DropColumn("dbo.Equipments", "ConveyorId");
            DropColumn("dbo.Equipments", "WorkshopId");
            DropTable("dbo.Workshops");
            DropTable("dbo.Conveyors");
        }
    }
}
