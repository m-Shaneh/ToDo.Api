﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Core;
using ToDo.Core.Models;

namespace ToDo.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);
            var userManager = services
                 .GetRequiredService<UserManager<ApplicationUser>>();
            await AddAdminRights(userManager);
        }

        private static async Task AddAdminRights(UserManager<ApplicationUser> userManager)
        {
            var adminExists = userManager.Users
                .Any(x => x.UserName == "shaneh");

            if (!adminExists)
            {
                var admin = new ApplicationUser
                {
                    Email = "shaneh@gmail.com",
                    UserName = "shaneh"
                };
                await userManager.CreateAsync(admin, "Shaneh1!");
                await userManager.AddToRoleAsync(
                    admin, Constants.AdministratorRole);
            }
        }

        private static async Task EnsureRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            var adminRoleAlreadyExists = await roleManager
                .RoleExistsAsync(Constants.AdministratorRole);

            var userRoleAlreadyExists = await roleManager
                .RoleExistsAsync(Constants.UserRole);

            if (!adminRoleAlreadyExists)
                await roleManager.CreateAsync(
                    new IdentityRole(Constants.AdministratorRole));

            if (!userRoleAlreadyExists)
                await roleManager.CreateAsync(
                    new IdentityRole(Constants.UserRole));
        }
    }
}
