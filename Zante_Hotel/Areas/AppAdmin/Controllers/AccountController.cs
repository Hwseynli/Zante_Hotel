using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public readonly string fileaddress = @"admin/images/user-images";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext dbContext, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _dbContext = dbContext;
        }
        public IActionResult Registr()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registr(RegistrVM newuser)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Name = newuser.Name,
                Email = newuser.Email,
                Surname = newuser.Surname,
                UserName = newuser.Username
            };
            //Age = newuser.Age,
            //                 Gender = newuser.Gender,

            if (newuser.Age < 0) return View();
            user.Age = newuser.Age;
            if (newuser.Gender == "Male" || newuser.Gender == "Female" || newuser.Gender == "Other")
            {
                user.Gender = newuser.Gender;
            }
            if (newuser.UserPhoto != null)
            {
                if (!newuser.UserPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                    return View();
                }
                if (!newuser.UserPhoto.CheckFileSize(2000))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 2000 kb-den boyuk olmamalidir");
                    return View();
                }
                user.UserImgUrl = await newuser.UserPhoto.CreateFileAsync(_env.WebRootPath, fileaddress);
            }
            var result = await _userManager.CreateAsync(user, newuser.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid) return View();
            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameOrEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameOrEmail);
                if (existed == null)
                {
                    ModelState.AddModelError(string.Empty, "Email, Username or Password is in correct!!!");
                    return View();
                }
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(existed, user.Password, user.IsRemember, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email, Username or Password is in correct!!!");
                return View();
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "You are Blocked");
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}


