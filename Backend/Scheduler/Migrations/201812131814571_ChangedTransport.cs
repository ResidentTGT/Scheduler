namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTransport : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transports", "Height");
            DropColumn("dbo.Transports", "Width");
            DropColumn("dbo.Transports", "Length");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transports", "Length", c => c.Int(nullable: false));
            AddColumn("dbo.Transports", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.Transports", "Height", c => c.Int(nullable: false));
        }
    }
}
