namespace DonorGateway.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GuestBoolDataTypesToNullable : DbMigration
    {
        public override void Up()
        {
            Sql("Update dbo.Guests SET IsAttending = NULL WHERE IsAttending = 0");
            Sql("Update dbo.Guests SET IsWaiting = NULL WHERE IsWaiting = 0");

            AlterColumn("dbo.Guests", "IsAttending", c => c.Boolean());
            AlterColumn("dbo.Guests", "IsWaiting", c => c.Boolean());
        }

        public override void Down()
        {
            Sql("Update dbo.Guests SET IsAttending = false WHERE IsAttending IS NULL");
            Sql("Update dbo.Guests SET IsWaiting = false WHERE IsWaiting IS NULL");

            AlterColumn("dbo.Guests", "IsWaiting", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Guests", "IsAttending", c => c.Boolean(nullable: false));
        }
    }
}
