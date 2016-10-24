using admin.web;
using DonorGateway.Data;
using DonorGateway.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;

[assembly: OwinStartup(typeof(Startup))]

namespace admin.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public static async void Seed()
        {
            //var context = new DataContext();
            //var userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            //var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

            //string[] roles = { "admin", "rsvp", "tax", "mailer", "user" };
            //foreach (string role in roles)
            //{
            //    if (!context.Roles.Any(r => r.Name == role))
            //    {
            //        await roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}

            //var users = roleManager.FindByName("tax-user").Users.ToList();
            //foreach (var user in users)
            //{
            //    await userManager.AddToRoleAsync(user.UserId, "tax");
            //}
            //foreach (var user in users)
            //{
            //    await userManager.RemoveFromRoleAsync(user.UserId, "tax-user");
            //}

            //var deletedRole = roleManager.FindByName("tax-user");
            //if (deletedRole != null) roleManager.Delete(deletedRole);

            //if (userManager.FindByName("admin") == null)
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "admin",
            //        FullName = "Administrator",
            //        Email = "mark.lawrence@splcenter.org"
            //    };
            //    userManager.Create(user, "password");
            //    await userManager.AddToRoleAsync(user.Id, "admin");
            //}
        }
    }
}