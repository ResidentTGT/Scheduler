namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyRoutesToDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Routes", "Id", "dbo.Details");
            DropForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes");
            DropIndex("dbo.Routes", new[] { "Id" });
            DropPrimaryKey("dbo.Routes");
            AddColumn("dbo.Routes", "DetailId", c => c.Int());
            AlterColumn("dbo.Routes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Routes", "Id");
            CreateIndex("dbo.Routes", "DetailId");
            AddForeignKey("dbo.Routes", "DetailId", "dbo.Details", "Id");
            AddForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.Routes", "DetailId", "dbo.Details");
            DropIndex("dbo.Routes", new[] { "DetailId" });
            DropPrimaryKey("dbo.Routes");
            AlterColumn("dbo.Routes", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Routes", "DetailId");
            AddPrimaryKey("dbo.Routes", "Id");
            CreateIndex("dbo.Routes", "Id");
            AddForeignKey("dbo.RouteOperations", "Route_Id", "dbo.Routes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Routes", "Id", "dbo.Details", "Id");
        }
    }
}
