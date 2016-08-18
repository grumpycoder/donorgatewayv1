namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DemographicChangeModel : DbMigration
    {
        public override void Up()
        {
            //DropTable("dbo.DemographicChanges");

            CreateTable(
                "dbo.DemographicChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Name = c.String(maxLength: 8000, unicode: false),
                        LookupId = c.String(maxLength: 8000, unicode: false),
                        FinderNumber = c.String(maxLength: 8000, unicode: false),
                        Street = c.String(maxLength: 8000, unicode: false),
                        Street2 = c.String(maxLength: 8000, unicode: false),
                        City = c.String(maxLength: 8000, unicode: false),
                        State = c.String(maxLength: 8000, unicode: false),
                        Zipcode = c.String(maxLength: 8000, unicode: false),
                        Email = c.String(maxLength: 8000, unicode: false),
                        Phone = c.String(maxLength: 8000, unicode: false),
                        CreatedDate = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 8000, unicode: false),
                        UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                        UpdatedBy = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DemographicChanges");
        }
    }
}
