namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuestStatusDataTypeToNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guests", "IsMailed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Guests", "IsAttending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Guests", "IsWaiting", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guests", "IsWaiting", c => c.Boolean());
            AlterColumn("dbo.Guests", "IsAttending", c => c.Boolean());
            AlterColumn("dbo.Guests", "IsMailed", c => c.Boolean());
        }
    }
}
