namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mailers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 8000, unicode: false),
                        StartDate = c.DateTime(storeType: "smalldatetime"),
                        EndDate = c.DateTime(storeType: "smalldatetime"),
                        CreatedDate = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 8000, unicode: false),
                        UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                        UpdatedBy = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mailers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 8000, unicode: false),
                        MiddleName = c.String(maxLength: 8000, unicode: false),
                        LastName = c.String(maxLength: 8000, unicode: false),
                        Suffix = c.String(maxLength: 8000, unicode: false),
                        Address = c.String(maxLength: 8000, unicode: false),
                        Address2 = c.String(maxLength: 8000, unicode: false),
                        Address3 = c.String(maxLength: 8000, unicode: false),
                        City = c.String(maxLength: 8000, unicode: false),
                        State = c.String(maxLength: 8000, unicode: false),
                        ZipCode = c.String(maxLength: 8000, unicode: false),
                        SourceCode = c.String(maxLength: 8000, unicode: false),
                        FinderNumber = c.String(maxLength: 8000, unicode: false),
                        Suppress = c.Boolean(),
                        CampaignId = c.Int(),
                        ReasonId = c.Int(),
                        CreatedDate = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 8000, unicode: false),
                        UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                        UpdatedBy = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.SuppressReasons", t => t.ReasonId)
                .Index(t => t.CampaignId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "dbo.SuppressReasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mailers", "ReasonId", "dbo.SuppressReasons");
            DropForeignKey("dbo.Mailers", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Mailers", new[] { "ReasonId" });
            DropIndex("dbo.Mailers", new[] { "CampaignId" });
            DropTable("dbo.SuppressReasons");
            DropTable("dbo.Mailers");
            DropTable("dbo.Campaigns");
        }
    }
}
