using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadanko3.Models;


namespace Zadanko3.Data
{
	public static class DbSeeder
	{
		public static void Seed(DataContext dataContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			if (!dataContext.Users.Any())
			{
				CreateUser(dataContext, roleManager, userManager).GetAwaiter().GetResult();
			}
			
		}
		private static async Task CreateUser(DataContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			string user_role = "User";
			string admin_role = "Admin";
			if (!await roleManager.RoleExistsAsync(user_role))
			{
				await roleManager.CreateAsync(new IdentityRole(user_role));
			}
			if (!await roleManager.RoleExistsAsync(admin_role))
			{
				await roleManager.CreateAsync(new IdentityRole(admin_role));
			}
		

			var role_user = new List<User.Role>
			{
				User.Role.User
			};
			#region createAdminUser
			var user_admin = new User()
			{
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = "Admin",
				role = User.Role.Admin
				
			};
			if (await userManager.FindByNameAsync(user_admin.UserName) == null)
			{
				await userManager.CreateAsync(user_admin, "Adminadmin");
				await userManager.AddToRoleAsync(user_admin, admin_role);
				await userManager.AddToRoleAsync(user_admin, user_role);
				user_admin.EmailConfirmed = true;
				user_admin.LockoutEnabled = false;
			}
			#endregion
			#region createUser
			var user_Kasprzyk = new User()
			{
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = "Mvozik",
				role = User.Role.User
			};
			if (await userManager.FindByNameAsync(user_Kasprzyk.UserName) == null)
			{
				await userManager.CreateAsync(user_Kasprzyk, "Mvozikmvozik");
				await userManager.AddToRoleAsync(user_Kasprzyk, user_role);

				user_Kasprzyk.EmailConfirmed = true;
				user_Kasprzyk.LockoutEnabled = false;
			}
			#endregion

			await dbContext.SaveChangesAsync();
		}
	}
}
