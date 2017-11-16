namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItemsCountFieldInOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderQuantums", "ItemsCountInOnePart", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderQuantums", "ItemsCountInOnePart");
        }
    }
}
