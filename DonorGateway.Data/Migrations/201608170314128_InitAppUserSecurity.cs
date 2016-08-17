namespace DonorGateway.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitAppUserSecurity : DbMigration
    {
        public override void Up()
        {
            Sql("SELECT * INTO Security.UserRoles2 FROM Security.UserRoles;");
            Sql("SELECT * INTO Security.UserLogins2 FROM Security.UserLogins;");
            Sql("SELECT * INTO Security.UserClaims2 FROM Security.UserClaims;");
            Sql("SELECT * INTO Security.Users2 FROM Security.Users;");
            Sql("SELECT * INTO Security.Roles2 FROM Security.Roles;");

            DropTable("Security.UserRoles");
            DropTable("Security.UserLogins");
            DropTable("Security.UserClaims");
            DropTable("Security.Users");
            DropTable("Security.Roles");

            CreateTable(
                "Security.Roles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128, unicode: false),
                    Name = c.String(nullable: false, maxLength: 256, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "Security.UserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128, unicode: false),
                    RoleId = c.String(nullable: false, maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Security.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "Security.Users",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128, unicode: false),
                    FullName = c.String(maxLength: 8000, unicode: false),
                    UserPhoto = c.Binary(),
                    UserPhotoType = c.String(maxLength: 8000, unicode: false),
                    Email = c.String(maxLength: 256, unicode: false),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(maxLength: 8000, unicode: false),
                    SecurityStamp = c.String(maxLength: 8000, unicode: false),
                    PhoneNumber = c.String(maxLength: 8000, unicode: false),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(storeType: "smalldatetime"),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "Security.UserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128, unicode: false),
                    ClaimType = c.String(maxLength: 8000, unicode: false),
                    ClaimValue = c.String(maxLength: 8000, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "Security.UserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128, unicode: false),
                    ProviderKey = c.String(nullable: false, maxLength: 128, unicode: false),
                    UserId = c.String(nullable: false, maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Security.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);


            Sql("INSERT INTO Security.Roles (Id, Name) " +
             "SELECT Id, Name FROM Security.Roles2;");

            Sql("INSERT INTO Security.Users (Id,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName) " +
                "SELECT Id,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName FROM Security.Users2; ");

            Sql("INSERT INTO Security.UserRoles (UserId,RoleId) " +
                "SELECT UserId,RoleId FROM Security.UserRoles2; ");

            Sql("INSERT INTO Security.UserLogins (LoginProvider,ProviderKey,UserId) " +
                "SELECT LoginProvider,ProviderKey,UserId FROM Security.UserLogins2; ");

            Sql("INSERT INTO Security.UserClaims (UserId, ClaimType, ClaimValue) " +
                "SELECT UserId, ClaimType, ClaimValue FROM Security.UserClaims2; ");

            DropTable("Security.UserRoles2");
            DropTable("Security.UserLogins2");
            DropTable("Security.UserClaims2");
            DropTable("Security.Users2");
            DropTable("Security.Roles2");

        }

        public override void Down()
        {
            DropForeignKey("Security.UserRoles", "UserId", "Security.Users");
            DropForeignKey("Security.UserLogins", "UserId", "Security.Users");
            DropForeignKey("Security.UserClaims", "UserId", "Security.Users");
            DropForeignKey("Security.UserRoles", "RoleId", "Security.Roles");
            DropIndex("Security.UserLogins", new[] { "UserId" });
            DropIndex("Security.UserClaims", new[] { "UserId" });
            DropIndex("Security.Users", "UserNameIndex");
            DropIndex("Security.UserRoles", new[] { "RoleId" });
            DropIndex("Security.UserRoles", new[] { "UserId" });
            DropIndex("Security.Roles", "RoleNameIndex");
            DropTable("Security.UserLogins");
            DropTable("Security.UserClaims");
            DropTable("Security.Users");
            DropTable("Security.UserRoles");
            DropTable("Security.Roles");
        }
    }
}
