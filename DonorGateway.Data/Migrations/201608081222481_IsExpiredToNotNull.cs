namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsExpiredToNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "IsCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "IsCancelled", c => c.Boolean());
        }
    }
}
