namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOrderDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PlannedBeginDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "PlannedEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PlannedEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "PlannedBeginDate", c => c.DateTime(nullable: false));
        }
    }
}
