namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conveyors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TransportTime = c.Time(nullable: false, precision: 7),
                        ReadjustingTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                        UsingTimeResource = c.Double(nullable: false),
                        LoadFactor = c.Double(nullable: false),
                        MaintenanceCost = c.Int(nullable: false),
                        WorkshopId = c.Int(),
                        ConveyorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workshops", t => t.WorkshopId)
                .ForeignKey("dbo.Conveyors", t => t.ConveyorId)
                .Index(t => t.WorkshopId)
                .Index(t => t.ConveyorId);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MainTime = c.Time(nullable: false, precision: 7),
                        AdditionalTime = c.Time(nullable: false, precision: 7),
                        RiggingCost = c.Int(nullable: false),
                        RiggingStorageCost = c.Int(nullable: false),
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
                "dbo.Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Cost = c.Int(),
                        IsPurchased = c.Boolean(),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Mass = c.Int(nullable: false),
                        WorkshopSequenceStr = c.String(),
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
                        ChildrenProductionItemsIds = c.String(),
                        OneItemIncome = c.Int(nullable: false),
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
                        ItemsCountInOnePart = c.Int(nullable: false),
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
                        PlannedBeginDate = c.DateTime(),
                        PlannedEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Workshops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportOperations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstWorkshopId = c.Int(nullable: false),
                        SecondWorkshopId = c.Int(nullable: false),
                        Distance = c.Single(nullable: false),
                        TransportId = c.Int(nullable: false),
                        OrderQuantumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderQuantums", t => t.OrderQuantumId, cascadeDelete: true)
                .ForeignKey("dbo.Transports", t => t.TransportId, cascadeDelete: true)
                .Index(t => t.TransportId)
                .Index(t => t.OrderQuantumId);
            
            CreateTable(
                "dbo.Transports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        LoadCapacity = c.Int(nullable: false),
                        UnloadingTime = c.Time(nullable: false, precision: 7),
                        AverageSpeed = c.Single(nullable: false),
                        IsFree = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        OperationsSequence = c.String(),
                        Cost = c.Int(nullable: false),
                        DetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailId)
                .Index(t => t.DetailId);
            
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
            DropForeignKey("dbo.Equipments", "ConveyorId", "dbo.Conveyors");
            DropForeignKey("dbo.Operations", "EquipmentId", "dbo.Equipments");
            DropForeignKey("dbo.Operations", "DetailId", "dbo.Details");
            DropForeignKey("dbo.RouteOperations", "Operation_Id", "dbo.Operations");
            DropForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.Routes", "DetailId", "dbo.Details");
            DropForeignKey("dbo.ProductionItems", "Detail_Id", "dbo.Details");
            DropForeignKey("dbo.ProductionItemQuantums", "ProductionItemId", "dbo.ProductionItems");
            DropForeignKey("dbo.TransportOperations", "TransportId", "dbo.Transports");
            DropForeignKey("dbo.TransportOperations", "OrderQuantumId", "dbo.OrderQuantums");
            DropForeignKey("dbo.OrderQuantums", "ProductionItemId", "dbo.ProductionItems");
            DropForeignKey("dbo.OrderReports", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderBlocks", "OrderReportId", "dbo.OrderReports");
            DropForeignKey("dbo.GroupBlocks", "OrderBlockId", "dbo.OrderBlocks");
            DropForeignKey("dbo.GroupBlocks", "WorkshopId", "dbo.Workshops");
            DropForeignKey("dbo.Equipments", "WorkshopId", "dbo.Workshops");
            DropForeignKey("dbo.DetailsBatchBlocks", "GroupBlockId", "dbo.GroupBlocks");
            DropForeignKey("dbo.DetailsBatchBlocks", "EquipmentId", "dbo.Equipments");
            DropForeignKey("dbo.OrderQuantums", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.ProductionItemQuantums", "DetailId", "dbo.Details");
            DropIndex("dbo.RouteOperations", new[] { "Operation_Id" });
            DropIndex("dbo.RouteOperations", new[] { "Route_Id" });
            DropIndex("dbo.Routes", new[] { "DetailId" });
            DropIndex("dbo.TransportOperations", new[] { "OrderQuantumId" });
            DropIndex("dbo.TransportOperations", new[] { "TransportId" });
            DropIndex("dbo.DetailsBatchBlocks", new[] { "EquipmentId" });
            DropIndex("dbo.DetailsBatchBlocks", new[] { "GroupBlockId" });
            DropIndex("dbo.GroupBlocks", new[] { "WorkshopId" });
            DropIndex("dbo.GroupBlocks", new[] { "OrderBlockId" });
            DropIndex("dbo.OrderBlocks", new[] { "OrderReportId" });
            DropIndex("dbo.OrderReports", new[] { "OrderId" });
            DropIndex("dbo.OrderQuantums", new[] { "OrderId" });
            DropIndex("dbo.OrderQuantums", new[] { "ProductionItemId" });
            DropIndex("dbo.ProductionItems", new[] { "Detail_Id" });
            DropIndex("dbo.ProductionItemQuantums", new[] { "ProductionItemId" });
            DropIndex("dbo.ProductionItemQuantums", new[] { "DetailId" });
            DropIndex("dbo.Operations", new[] { "DetailId" });
            DropIndex("dbo.Operations", new[] { "EquipmentId" });
            DropIndex("dbo.Equipments", new[] { "ConveyorId" });
            DropIndex("dbo.Equipments", new[] { "WorkshopId" });
            DropTable("dbo.RouteOperations");
            DropTable("dbo.Routes");
            DropTable("dbo.Transports");
            DropTable("dbo.TransportOperations");
            DropTable("dbo.Workshops");
            DropTable("dbo.DetailsBatchBlocks");
            DropTable("dbo.GroupBlocks");
            DropTable("dbo.OrderBlocks");
            DropTable("dbo.OrderReports");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderQuantums");
            DropTable("dbo.ProductionItems");
            DropTable("dbo.ProductionItemQuantums");
            DropTable("dbo.Details");
            DropTable("dbo.Operations");
            DropTable("dbo.Equipments");
            DropTable("dbo.Conveyors");
        }
    }
}
