namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Cost = c.Int(),
                        IsPurchased = c.Boolean(),
                        RouteId = c.Int(),
                        Route_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.Route_Id)
                .Index(t => t.Route_Id);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MainTime = c.Time(nullable: false, precision: 7),
                        AdditionalTime = c.Time(nullable: false, precision: 7),
                        Type = c.Int(nullable: false),
                        EquipmentId = c.Int(nullable: false),
                        DetailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailId, cascadeDelete: true)
                .ForeignKey("dbo.Equipments", t => t.EquipmentId, cascadeDelete: true)
                .Index(t => t.EquipmentId)
                .Index(t => t.DetailId);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductionItemQuantums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailId = c.Int(nullable: false),
                        ProductionItemId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailId, cascadeDelete: true)
                .ForeignKey("dbo.ProductionItems", t => t.ProductionItemId, cascadeDelete: true)
                .Index(t => t.DetailId)
                .Index(t => t.ProductionItemId);
            
            CreateTable(
                "dbo.ProductionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ParentProductionItemId = c.Int(),
                        IsNode = c.Boolean(nullable: false),
                        Detail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.Detail_Id)
                .Index(t => t.Detail_Id);
            
            CreateTable(
                "dbo.OrderQuantums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.ProductionItems", t => t.ProductionItemId, cascadeDelete: true)
                .Index(t => t.ProductionItemId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        State = c.Int(nullable: false),
                        PlannedBeginDate = c.DateTime(nullable: false),
                        PlannedEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RouteOperations",
                c => new
                    {
                        Route_Id = c.Int(nullable: false),
                        Operation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Route_Id, t.Operation_Id })
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .ForeignKey("dbo.Operations", t => t.Operation_Id, cascadeDelete: true)
                .Index(t => t.Route_Id)
                .Index(t => t.Operation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.ProductionItems", "Detail_Id", "dbo.Details");
            DropForeignKey("dbo.ProductionItemQuantums", "ProductionItemId", "dbo.ProductionItems");
            DropForeignKey("dbo.OrderQuantums", "ProductionItemId", "dbo.ProductionItems");
            DropForeignKey("dbo.OrderQuantums", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.ProductionItemQuantums", "DetailId", "dbo.Details");
            DropForeignKey("dbo.RouteOperations", "Operation_Id", "dbo.Operations");
            DropForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.Operations", "EquipmentId", "dbo.Equipments");
            DropForeignKey("dbo.Operations", "DetailId", "dbo.Details");
            DropIndex("dbo.RouteOperations", new[] { "Operation_Id" });
            DropIndex("dbo.RouteOperations", new[] { "Route_Id" });
            DropIndex("dbo.OrderQuantums", new[] { "OrderId" });
            DropIndex("dbo.OrderQuantums", new[] { "ProductionItemId" });
            DropIndex("dbo.ProductionItems", new[] { "Detail_Id" });
            DropIndex("dbo.ProductionItemQuantums", new[] { "ProductionItemId" });
            DropIndex("dbo.ProductionItemQuantums", new[] { "DetailId" });
            DropIndex("dbo.Operations", new[] { "DetailId" });
            DropIndex("dbo.Operations", new[] { "EquipmentId" });
            DropIndex("dbo.Details", new[] { "Route_Id" });
            DropTable("dbo.RouteOperations");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderQuantums");
            DropTable("dbo.ProductionItems");
            DropTable("dbo.ProductionItemQuantums");
            DropTable("dbo.Routes");
            DropTable("dbo.Equipments");
            DropTable("dbo.Operations");
            DropTable("dbo.Details");
        }
    }
}
