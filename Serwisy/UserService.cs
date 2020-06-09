using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadanko3.Models;
using Zadanko3.Serwisy.IServices;

namespace Zadanko3.Serwisy
{
	public class UserService : IUserService
	{
		private readonly DataContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<User> _signInManager;
		public UserService(DataContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}
		public async Task<bool> Login(string username, string password)
		{
			if (username == null)
			{
				return false;
			}
			var user =  await _userManager.FindByNameAsync(username); // szukanie user'a
			var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, true); // logowanie
			if (result.Succeeded)
			{
				return true;
			}

			return false;

		}

		public async Task<bool> Register(string username, string password)
		{
			string user_role = "User";

			if (! await _roleManager.RoleExistsAsync(user_role))
			{
				await _roleManager.CreateAsync(new IdentityRole(user_role));
			}
			List<User.Role> roles = new List<User.Role> {
				User.Role.User
			};
			User user = new User
			{
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = username,
				role = User.Role.User
			};

			await _userManager.CreateAsync(user, password);
			await _userManager.AddToRoleAsync(user, user_role);

			
			user.LockoutEnabled = false;

			if (await _userManager.FindByNameAsync(user.UserName) != null)
				return true;
			else
				return false;
		}

		
	}
}
