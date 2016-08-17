namespace DonorGateway.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EventGuestTemplateModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "TemplateId" });
            DropTable("dbo.Templates");
            DropTable("dbo.Guests");
            DropTable("dbo.Events");

            CreateTable(
                "dbo.Events",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 8000, unicode: false),
                    DisplayName = c.String(maxLength: 8000, unicode: false),
                    EventCode = c.String(maxLength: 8000, unicode: false),
                    Speaker = c.String(maxLength: 8000, unicode: false),
                    Venue = c.String(maxLength: 8000, unicode: false),
                    Street = c.String(maxLength: 8000, unicode: false),
                    City = c.String(maxLength: 8000, unicode: false),
                    State = c.String(maxLength: 8000, unicode: false),
                    Zipcode = c.String(maxLength: 8000, unicode: false),
                    Capacity = c.Int(nullable: false),
                    StartDate = c.DateTime(storeType: "smalldatetime"),
                    EndDate = c.DateTime(storeType: "smalldatetime"),
                    VenueOpenDate = c.DateTime(storeType: "smalldatetime"),
                    RegistrationCloseDate = c.DateTime(storeType: "smalldatetime"),
                    TicketAllowance = c.Int(),
                    IsCancelled = c.Boolean(nullable: false),
                    TemplateId = c.Int(),
                    GuestWaitingCount = c.Int(nullable: false),
                    GuestAttendanceCount = c.Int(nullable: false),
                    TicketMailedCount = c.Int(nullable: false),
                    CreatedDate = c.DateTime(storeType: "smalldatetime"),
                    CreatedBy = c.String(maxLength: 8000, unicode: false),
                    UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                    UpdatedBy = c.String(maxLength: 8000, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.TemplateId)
                .Index(t => t.TemplateId);

            CreateTable(
                "dbo.Guests",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    LookupId = c.String(maxLength: 8000, unicode: false),
                    FinderNumber = c.String(maxLength: 8000, unicode: false),
                    ConstituentType = c.String(maxLength: 8000, unicode: false),
                    SourceCode = c.String(maxLength: 8000, unicode: false),
                    InteractionId = c.String(maxLength: 8000, unicode: false),
                    MembershipYear = c.String(maxLength: 8000, unicode: false),
                    LeadershipCouncil = c.Boolean(),
                    InsideSalutation = c.String(maxLength: 8000, unicode: false),
                    OutsideSalutation = c.String(maxLength: 8000, unicode: false),
                    HouseholdSalutation1 = c.String(maxLength: 8000, unicode: false),
                    HouseholdSalutation2 = c.String(maxLength: 8000, unicode: false),
                    HouseholdSalutation3 = c.String(maxLength: 8000, unicode: false),
                    EmailSalutation = c.String(maxLength: 8000, unicode: false),
                    Name = c.String(maxLength: 8000, unicode: false),
                    Email = c.String(maxLength: 8000, unicode: false),
                    Phone = c.String(maxLength: 8000, unicode: false),
                    Address = c.String(maxLength: 8000, unicode: false),
                    Address2 = c.String(maxLength: 8000, unicode: false),
                    Address3 = c.String(maxLength: 8000, unicode: false),
                    City = c.String(maxLength: 8000, unicode: false),
                    State = c.String(maxLength: 8000, unicode: false),
                    StateName = c.String(maxLength: 8000, unicode: false),
                    Zipcode = c.String(maxLength: 8000, unicode: false),
                    Country = c.String(maxLength: 8000, unicode: false),
                    TicketCount = c.Int(),
                    IsMailed = c.Boolean(nullable: false),
                    IsAttending = c.Boolean(),
                    IsWaiting = c.Boolean(),
                    ResponseDate = c.DateTime(storeType: "smalldatetime"),
                    MailedDate = c.DateTime(storeType: "smalldatetime"),
                    WaitingDate = c.DateTime(storeType: "smalldatetime"),
                    MailedBy = c.String(maxLength: 8000, unicode: false),
                    EventId = c.Int(nullable: false),
                    ActualDate = c.String(maxLength: 8000, unicode: false),
                    ExpectedDate = c.String(maxLength: 8000, unicode: false),
                    Comment = c.String(maxLength: 8000, unicode: false),
                    ResponseType = c.String(maxLength: 8000, unicode: false),
                    SPLCComment = c.String(maxLength: 8000, unicode: false),
                    Status = c.String(maxLength: 8000, unicode: false),
                    ContactMethod = c.String(maxLength: 8000, unicode: false),
                    Summary = c.String(maxLength: 8000, unicode: false),
                    Category = c.String(maxLength: 8000, unicode: false),
                    SubCategory = c.String(maxLength: 8000, unicode: false),
                    Owner = c.String(maxLength: 8000, unicode: false),
                    CreatedDate = c.DateTime(storeType: "smalldatetime"),
                    CreatedBy = c.String(maxLength: 8000, unicode: false),
                    UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                    UpdatedBy = c.String(maxLength: 8000, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);

            CreateTable(
                "dbo.Templates",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 8000, unicode: false),
                    HeaderText = c.String(maxLength: 8000, unicode: false),
                    BodyText = c.String(maxLength: 8000, unicode: false),
                    FooterText = c.String(maxLength: 8000, unicode: false),
                    FAQText = c.String(maxLength: 8000, unicode: false),
                    YesResponseText = c.String(maxLength: 8000, unicode: false),
                    NoResponseText = c.String(maxLength: 8000, unicode: false),
                    WaitingResponseText = c.String(maxLength: 8000, unicode: false),
                    CancelledEventText = c.String(maxLength: 8000, unicode: false),
                    ExpiredEventText = c.String(maxLength: 8000, unicode: false),
                    Image = c.Binary(),
                    MimeType = c.String(maxLength: 8000, unicode: false),
                    CreatedDate = c.DateTime(storeType: "smalldatetime"),
                    CreatedBy = c.String(maxLength: 8000, unicode: false),
                    UpdatedDate = c.DateTime(storeType: "smalldatetime"),
                    UpdatedBy = c.String(maxLength: 8000, unicode: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Events", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "TemplateId" });
            DropTable("dbo.Templates");
            DropTable("dbo.Guests");
            DropTable("dbo.Events");
        }
    }
}
