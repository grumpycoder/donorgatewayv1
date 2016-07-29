using DonorGateway.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Diagnostics;

namespace DonorGateway.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DefaultConnection")
        {
            Database.Log = msg => Debug.WriteLine(msg);
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Properties<string>().Configure(c => c.HasColumnType("varchar"));
            builder.Properties<DateTime>().Configure(c => c.HasColumnType("smalldatetime"));

            builder.Entity<ApplicationUser>().ToTable("Users", "Security");
            builder.Entity<IdentityUserRole>().ToTable("UserRoles", "Security");
            builder.Entity<IdentityUserClaim>().ToTable("UserClaims", "Security");
            builder.Entity<IdentityUserLogin>().ToTable("UserLogins", "Security");
            builder.Entity<IdentityRole>().ToTable("Roles", "Security");


        }

    }
}