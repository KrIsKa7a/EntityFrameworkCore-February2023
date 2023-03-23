//using Blog.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Blog.Core.Models.User;

namespace Blog.Controllers
{
    using System.Text;
    using System.Threading.Tasks;
    using Services.Data;
    using Web.ViewModels.ApplicationUser;

    //[Authorize]
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService userService;

        public ApplicationUserController(IApplicationUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        //[AllowAnonymous]
        public IActionResult Register()
        {
            return this.View(new RegisterUserInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool usernameOrEmailTaken = await this.userService.UsernameExistsAsync(model.Username) ||
                                        await this.userService.EmailExistsAsync(model.Email);
            if (usernameOrEmailTaken)
            {
                return this.RedirectToAction("Login");
            }

            if (model.Password != model.PasswordConfirmation)
            {
                return this.View(model);
            }

            await this.userService.CreateUserAsync(model);

            return this.RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = await this.userService.GetIdByUsernameAsync(model.Username);
            if (userId == null)
            {
                ModelState.AddModelError("Username", "Invalid username!");
                return this.View(model);
            }

            bool isLoggedIn = 
                await this.userService.ValidateLoginInfoAsync(model);
            if (!isLoggedIn)
            {
                ModelState.AddModelError("Password", "Invalid password!");
                return this.View(model);
            }

            HttpContext.Session.Set("userId", Encoding.UTF8.GetBytes(userId));

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
