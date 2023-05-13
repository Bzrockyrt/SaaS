using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaaS.Domain.Models;
using SaaS.Domain.ViewModels;

namespace SaaS.Areas.WebSite.Controllers
{
    [Area("WebSite")]
    public class HomePageController : Controller
    {
        private readonly ILogger<LoginViewModel> logger;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public HomePageController(ILogger<LoginViewModel> logger,
            SignInManager<IdentityUser> signinManager, 
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.signInManager = signinManager;
            this.userManager = userManager;
        }

        public IActionResult HomePage()
        {
            return View();
        }

        #region Login
        //GET
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Utilisateur inexistant");
                        return View(loginViewModel);
                    }
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    /*if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }*/
                    /*if (result.IsLockedOut)
                    {
                        logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }*/
                    else
                    {
                        ModelState.AddModelError("", "Informations invalides");
                    }

                }
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de la connexion");
                return View(loginViewModel);
            }
        }
        #endregion

        #region Signup
        //GET
        [HttpGet]
        public async Task<IActionResult> Signup(string returnUrl = null)
        {
            SignupViewModel signupViewModel = new SignupViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(signupViewModel);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel signupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(signupViewModel.Password != signupViewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError(string.Empty, "Les mots de passes ne correspondent pas");
                        return View(signupViewModel);
                    }

                    var user = new User()
                    {
                        UserName = signupViewModel.Email,
                        Email = signupViewModel.Email,
                        Password = signupViewModel.Password,
                    };

                    var result = await userManager.CreateAsync(user, signupViewModel.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                return View(signupViewModel);
            }
            catch (Exception ex)
            {
                return View(signupViewModel);
            }
        }
        #endregion
    }
}
