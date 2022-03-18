using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
        }

        #region Register
        public IActionResult Register() => View(new RegisterUserViewModel());
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var user = new User
            {
                UserName = Model.UserName
            };
            var register_result = await _UserManager.CreateAsync(user, Model.Password);
            if (register_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in register_result.Errors)
               ModelState.AddModelError("", error.Description);
            return View(Model);
        }
        #endregion

        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel {ReturnUrl = ReturnUrl });
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var loginn_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName, 
                Model.Password, 
                Model.RememberMe,
#if DEBUG
                false
#else
                true
#endif

                );
            if (loginn_result.Succeeded)
            {
                //return Redirect(Model.ReturnUrl); //НЕЛЬЗЯ!! НЕ БЕЗОПАСНО!!!!
                #region
                //if (Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //else
                //    return RedirectToAction("Index", "Home");
                #endregion
                return LocalRedirect(Model.ReturnUrl ?? "/"); //аналог вышенаписанного региона, но одной строчкой
            }
            ModelState.AddModelError("", "Ошибка в имени пользователя или в пароле");
            return View(Model);

        }
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
