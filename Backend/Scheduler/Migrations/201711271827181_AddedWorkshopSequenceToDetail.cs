namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWorkshopSequenceToDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Details", "WorkshopSequenceStr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Details", "WorkshopSequenceStr");
        }
    }
}
