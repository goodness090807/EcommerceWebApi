using EcommerceWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Data
{
    /// <summary>
    /// 建立初始資料
    /// </summary>
    public class SeedData
    {
        /// <summary>
        /// 新建的資料庫建立管理者的資料
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public static async Task SeedManager(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole{ Name = "Member" },
                new IdentityRole{ Name = "Admin" },
                new IdentityRole{ Name = "Moderator" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                FirstName = "管理者"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
    }
}
