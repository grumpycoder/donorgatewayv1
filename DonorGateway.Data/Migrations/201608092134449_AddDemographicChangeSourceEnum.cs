namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDemographicChangeSourceEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DemographicChanges", "Source", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DemographicChanges", "Source");
        }
    }
}
