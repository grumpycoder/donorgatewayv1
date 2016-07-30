using admin.web;
using DonorGateway.Data;
using DonorGateway.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

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
            var context = new DataContext();
            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

            if (roleManager.FindByName("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (roleManager.FindByName("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (userManager.FindByName("admin") == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "mark.lawrence@splcenter.org"
                };
                userManager.Create(user, "password");
                await userManager.AddToRoleAsync(user.Id, "admin");
            }
        }
    }
}