using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanko3.Models;

using Zadanko3.Serwisy.IServices;

namespace Zadanko3.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
		private readonly IUserService _userService;

		public UserController(DataContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IUserService userService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
			_userService = userService;
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}
	
		[HttpPost]
		public async Task<IActionResult> Register([Bind("Name,Password")] UserView userView)
		{

			if (ModelState.IsValid)

			{

				var succes = await _userService.Register(userView.Name, userView.Password);

				if (succes == false)
				{
					return BadRequest("Złe dane");
				}

				else
				{
					await _userService.Login(userView.Name, userView.Password);
					return RedirectToAction("Index", "Losts");
				}
			}
			else
				return View(userView);

		}

		[HttpPost]
		public async Task<IActionResult> Login([Bind("Name,Password")] UserView userView)
		{

			if (userView == null || userView.Password==null)
			{
				return View();

			}
			else
			{

			
			var user = await _userManager.FindByNameAsync(userView.Name); // szukanie user'a
			if (user ==null )
			{ return View(); }
			else { 
			var result = await _signInManager.PasswordSignInAsync(user.UserName, userView.Password, true, true); // logowanie
					
					if (result.Succeeded)
			{
						return RedirectToAction("Index", "Losts");
			}
			}
			return View();

			}
		}


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}



	}
}