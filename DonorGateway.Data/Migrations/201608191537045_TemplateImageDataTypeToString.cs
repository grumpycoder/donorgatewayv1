namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateImageDataTypeToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Templates", "Image", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Templates", "Image", c => c.Binary());
        }
    }
}
