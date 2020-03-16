using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndetityServer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IndetityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new Login
            { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login vm)
        {
            //check if the model is valid

            var result = await _signInManager.PasswordSignInAsync(
                vm.Username, vm.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new Registration
            { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Register(Registration vm)
        {
            //check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new IdentityUser(vm.Username);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(vm.ReturnUrl);
            }

            return View();
        }
    }
}