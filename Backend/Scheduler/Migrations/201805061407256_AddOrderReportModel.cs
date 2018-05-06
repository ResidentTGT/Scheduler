namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderReportModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderReportId = c.Int(nullable: false),
                        ProductionItemId = c.Int(nullable: false),
                        ProductionItemsCount = c.Int(nullable: false),
                        ProductionItemsName = c.String(),
                        StartTime = c.Long(nullable: false),
                        Duration = c.Long(nullable: false),
                        IsMachining = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderReports", t => t.OrderReportId, cascadeDelete: true)
                .Index(t => t.OrderReportId);
            
            CreateTable(
                "dbo.GroupBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupIndex = c.Int(nullable: false),
                        OrderBlockId = c.Int(nullable: false),
                        StartTime = c.Long(nullable: false),
                        Duration = c.Long(nullable: false),
                        WorkshopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workshops", t => t.WorkshopId, cascadeDelete: true)
                .ForeignKey("dbo.OrderBlocks", t => t.OrderBlockId, cascadeDelete: true)
                .Index(t => t.OrderBlockId)
                .Index(t => t.WorkshopId);
            
            CreateTable(
                "dbo.DetailsBatchBlocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailId = c.Int(nullable: false),
                        DetailName = c.String(),
                        DetailsCount = c.Int(nullable: false),
                        GroupBlockId = c.Int(nullable: false),
                        StartTime = c.Long(nullable: false),
                        Duration = c.Long(nullable: false),
                        EquipmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Equipments", t => t.EquipmentId, cascadeDelete: true)
                .ForeignKey("dbo.GroupBlocks", t => t.GroupBlockId, cascadeDelete: true)
                .Index(t => t.GroupBlockId)
                .Index(t => t.EquipmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderReports", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderBlocks", "OrderReportId", "dbo.OrderReports");
            DropForeignKey("dbo.GroupBlocks", "OrderBlockId", "dbo.OrderBlocks");
            DropForeignKey("dbo.GroupBlocks", "WorkshopId", "dbo.Workshops");
            DropForeignKey("dbo.DetailsBatchBlocks", "GroupBlockId", "dbo.GroupBlocks");
            DropForeignKey("dbo.DetailsBatchBlocks", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.DetailsBatchBlocks", new[] { "EquipmentId" });
            DropIndex("dbo.DetailsBatchBlocks", new[] { "GroupBlockId" });
            DropIndex("dbo.GroupBlocks", new[] { "WorkshopId" });
            DropIndex("dbo.GroupBlocks", new[] { "OrderBlockId" });
            DropIndex("dbo.OrderBlocks", new[] { "OrderReportId" });
            DropIndex("dbo.OrderReports", new[] { "OrderId" });
            DropTable("dbo.DetailsBatchBlocks");
            DropTable("dbo.GroupBlocks");
            DropTable("dbo.OrderBlocks");
            DropTable("dbo.OrderReports");
        }
    }
}
