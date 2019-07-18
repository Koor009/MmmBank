using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using MmmBankDb.Models;
using System;

namespace MmmBank.Models
{
    public class PersoneRoles : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole { Name = "admin" });
            roleManager.Create(new IdentityRole { Name = "user"});
            roleManager.Create(new IdentityRole { Name = "moderator" });

            var admin = new ApplicationUser { Email = "koor0091@gmail.com", UserName = "koor0091@gmail.com", DateOfBirth = DateTime.UtcNow };
            string password = "ad46D_ew2r3w";
            var result = userManager.Create(admin, password);

            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, "admin");
            }

            base.Seed(context);
        }
    }
}