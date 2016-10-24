namespace DonorGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSupressionReasonFieldName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SuppressReasons", "Name", c => c.String(maxLength: 8000, unicode: false));
            DropColumn("dbo.SuppressReasons", "Reason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SuppressReasons", "Reason", c => c.String(maxLength: 8000, unicode: false));
            DropColumn("dbo.SuppressReasons", "Name");
        }
    }
}
