namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTransport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Details", "Height", c => c.Int(nullable: false));
            AddColumn("dbo.Details", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.Details", "Length", c => c.Int(nullable: false));
            AddColumn("dbo.Details", "Mass", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Details", "Mass");
            DropColumn("dbo.Details", "Length");
            DropColumn("dbo.Details", "Width");
            DropColumn("dbo.Details", "Height");
        }
    }
}
