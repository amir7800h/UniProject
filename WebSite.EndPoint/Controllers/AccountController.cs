using Application.Sevices.GoogleRecaptchaService;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.Utility.Filters;
using WebSite.EndPoint.Models.ViewModels.User;

namespace WebSite.EndPoint.Controllers
{
    [ServiceFilter(typeof(SaveVisitorFilter))]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            //Check Password and RePassword

            User newUser = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
            };

            var result = _userManager.CreateAsync(newUser, model.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }
            string erros = "";
            foreach (var item in result.Errors)
            {
                
                //erros = item.Description.ToString() + erros;
                ModelState.AddModelError("", item.Description.ToString());
            }
            ViewBag.result = erros;

            return View(model);
        }
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            string googleResponse = HttpContext.Request.Form["g-Recaptcha-Response"];

            GoogleRecaptcha googleRecaptcha = new GoogleRecaptcha();
            if(googleRecaptcha.Verify(googleResponse) == false)
            {
                ModelState.AddModelError("", "لطفا بروی من ربات نیستم کلیک کنید");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userManager.FindByNameAsync(model.Email).Result;
            if(user == null)
            {
                ModelState.AddModelError("","یوزر یافت نشد");
                return View(model);
            }
            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(user,
                model.Password, model.IsPersistent, true).Result;

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                //
            }
            ModelState.AddModelError("", "رمز عبور یا کلمه کاربری اشتباه است");
            return View(model);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return Redirect("/");
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Message"] = "Salam ....";
            return View();
        }
    }
}
