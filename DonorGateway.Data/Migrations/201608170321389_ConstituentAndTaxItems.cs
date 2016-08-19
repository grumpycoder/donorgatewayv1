namespace DonorGateway.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ConstituentAndTaxItems : DbMigration
    {
        public override void Up()
        {
            Sql("SELECT Id, Name, LookupId, FinderNumber, Street, Street2, City, State, Zipcode, Email, Phone, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy INTO dbo.Constituents2 FROM dbo.Constituents;");
            Sql("SELECT * INTO dbo.TaxItems2 FROM dbo.TaxItems;");

            DropForeignKey("dbo.TaxItems", "FK_dbo.TaxItems_dbo.Constituents_ConstituentId");

            DropTable("dbo.Constituents");
            DropTable("dbo.TaxItems");

            CreateTable(
                "dbo.Constituents",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
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

            CreateTable(
                "dbo.TaxItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ConstituentId = c.Int(nullable: false),
                    TaxYear = c.Int(nullable: false),
                    DonationDate = c.DateTime(storeType: "smalldatetime"),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsUpdated = c.Boolean(),
                    CreatedDate = c.DateTime(storeType: "smalldatetime"),
                    CreatedBy = c.String(maxLength: 8000, unicode: false),
                    UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                    UpdatedBy = c.String(maxLength: 8000, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituents", t => t.ConstituentId, cascadeDelete: true)
                .Index(t => t.ConstituentId);

            Sql("SET IDENTITY_INSERT dbo.Constituents ON; " +
                "INSERT INTO dbo.Constituents (Id, Name, LookupId, FinderNumber, Street, Street2, City, State, Zipcode, Email, Phone, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) " +
                "SELECT Id, Name, LookupId, FinderNumber, Street, Street2, City, State, Zipcode, Email, Phone, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM dbo.Constituents2; " +
                " SET IDENTITY_INSERT dbo.Constituents OFF; ");

            Sql("SET IDENTITY_INSERT dbo.TaxItems ON; " +
               "INSERT INTO dbo.TaxItems (Id, ConstituentId,TaxYear,DonationDate,Amount,IsUpdated,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy) " +
               "SELECT Id, ConstituentId,TaxYear,DonationDate,Amount,IsUpdated,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy FROM dbo.TaxItems2; " +
               " SET IDENTITY_INSERT dbo.TaxItems OFF; ");

            DropTable("dbo.Constituents2");
            DropTable("dbo.TaxItems2");

        }

        public override void Down()
        {
            DropForeignKey("dbo.TaxItems", "ConstituentId", "dbo.Constituents");
            DropIndex("dbo.TaxItems", new[] { "ConstituentId" });
            DropTable("dbo.TaxItems");
            DropTable("dbo.Constituents");
        }
    }
}
