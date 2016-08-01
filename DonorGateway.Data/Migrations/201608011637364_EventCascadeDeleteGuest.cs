namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCascadeDeleteGuest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "EventId" });
            AlterColumn("dbo.Guests", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Guests", "EventId");
            AddForeignKey("dbo.Guests", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "EventId" });
            AlterColumn("dbo.Guests", "EventId", c => c.Int());
            CreateIndex("dbo.Guests", "EventId");
            AddForeignKey("dbo.Guests", "EventId", "dbo.Events", "Id");
        }
    }
}
