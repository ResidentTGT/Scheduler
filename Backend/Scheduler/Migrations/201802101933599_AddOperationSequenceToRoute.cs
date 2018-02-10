namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperationSequenceToRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "OperationsSequence", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "OperationsSequence");
        }
    }
}
