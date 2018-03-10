namespace Scheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChildrenProductionItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductionItems", "ChildrenProductionItemsIds", c => c.String());
            DropColumn("dbo.ProductionItems", "ParentProductionItemId");
            DropColumn("dbo.ProductionItems", "IsNode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductionItems", "IsNode", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductionItems", "ParentProductionItemId", c => c.Int());
            DropColumn("dbo.ProductionItems", "ChildrenProductionItemsIds");
        }
    }
}
