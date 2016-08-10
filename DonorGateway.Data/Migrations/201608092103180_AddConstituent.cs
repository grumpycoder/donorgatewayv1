namespace DonorGateway.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddConstituent : DbMigration
    {
        public override void Up()
        {

            DropForeignKey("dbo.TaxItems", "ConstituentId", "dbo.Constituents");
            DropIndex("dbo.Constituents", new[] { "PK_dbo.Constituents" });
            RenameTable(name: "dbo.Constituents", newName: "Constituents2");

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

            Sql("TRUNCATE TABLE dbo.Constituents");
            Sql("SET IDENTITY_INSERT dbo.Constituents ON; " +
                "INSERT INTO dbo.Constituents (Id, Name, LookupId, FinderNumber, Street, Street2, City, State, Zipcode, Email, Phone, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) " +
                "SELECT Id, Name, LookupId, FinderNumber, Street, Street2, City, State, Zipcode, Email, Phone, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM dbo.Constituents2; " +
                " SET IDENTITY_INSERT dbo.Constituents ON; ");
            Sql("DROP TABLE dbo.Constituents");
        }

        public override void Down()
        {
            DropTable("dbo.Constituents");
        }
    }
}
