using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public async static Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()){
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "bob",
                        LastName = "Bobbity",
                        City = "New York",
                        Street = "10 The Street",
                        State = "NY",
                        ZipCode = "9020"
                    }

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
            
        }
    }
}