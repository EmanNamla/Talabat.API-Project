using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var Users = new AppUser
                {
                    DisplayName = "EmanNamla",
                    Email = "Emanrnamla222@gmail.com",
                    UserName = "EmanRnamla",
                    PhoneNumber = "01023971523"

                };
               await userManager.CreateAsync(Users,"Pa$$w0rd");
            }

        }
    }
}
