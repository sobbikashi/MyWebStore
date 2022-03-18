using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(
            UserManager<User> UserManager, 
            SignInManager<User> SignInManager, 
            ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        #region Register
        public IActionResult Register() => View(new RegisterUserViewModel());
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);
            _Logger.LogInformation("Регистрация нового пользователя {0}", Model.UserName);
            var user = new User
            {
                UserName = Model.UserName
            };
            var register_result = await _UserManager.CreateAsync(user, Model.Password);
            if (register_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);
                _Logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in register_result.Errors)
               ModelState.AddModelError("", error.Description);
            _Logger.LogWarning("Ошибка при регистрации пользователя {0} в системе: {1}", 
                Model.UserName,
                string.Join(", ", register_result.Errors.Select(err => err.Description)));
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
