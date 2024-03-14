using EduGate.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy.Identity
{
    public static class AppDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
           if(_userManager.Users.Count() == 0)
           {
                var user = new AppUser()
                {
                    DisplayName = "Mudar Abd Elhady",
                    Email = "42020290@hti.com",
                    UserName = "42020290",
                    PhoneNumber = "1234567890",
                    PictureUrl = "imgs/students/mudar.jpeg"
                };
                
                await _userManager.CreateAsync(user, "Pa$$w0rd");
           }   
        }
    }
}
