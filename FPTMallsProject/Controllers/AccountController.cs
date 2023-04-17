using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FPTMallsProject.Models;
using FPTMallsProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signinManager;

		private readonly AppDbContext db;


		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext db)
        {
            _userManager = userManager;
            _signinManager = signInManager;
			this.db = db;
		}

		[HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    /*await _userManager.AddToRoleAsync(user, "Admin");*/

                    await _signinManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Movie");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string returnUrl, LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);


                if (result.Succeeded)
                {
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin"); 
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("Index", "Movie"); 
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Movie");
        }

    }
}
