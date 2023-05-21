using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain.Models.Account;
using SaaS.Domain.Models;
using SaaS.ViewModels.Application.Connection;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class ConnectionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<User> signInManager;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;
        private readonly UserManager<User> userManager;

        public ConnectionController(IUnitOfWork unitOfWork,
            SignInManager<User> signInManager,
            TenantService tenantService,
            IOptions<TenantSettings> options,
            IUserStore<User> userStore,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
            this._userStore = userStore;
            this.userManager = userManager;
            this._emailStore = GetEmailStore();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region CompanyConnection
        //GET
        [HttpGet]
        public async Task<IActionResult> CompanyConnection()
        {
            //Si le cookie de connexion n'est pas vide (entreprise précédente), alors rediriger directement vers la page de connexion
            var company = this.tenantService.GetTenant();
            if (company != null)
            {
                ViewBag.CurrentCompany = new TenantSiteModel
                {
                    Key = this.tenantService.GetTenantCode(),
                    Logo = company.logo,
                    Name = company.name
                };

                return RedirectToAction("Connection", "HomePage");
            }
            return View();
        }

        //SET
        [HttpPost]
        public async Task<IActionResult> CompanyConnection(CompanyConnectionViewModel companyConnectionViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Je vérifie si le code entreprise correspond à une entreprise existante
                    var company = this.unitOfWork.Company.Get(c => c.CompanyCode == companyConnectionViewModel.CompanyCode);
                    if (company is null)
                    {
                        //Si 'company' est null, l'entreprise n'existe pas
                        return View("L'entreprise n'existe pas");
                    }
                    else
                    {
                        //Si 'company' n'est pas null, je vérifie qu'elle possède bien une chaîne de connexion
                        //(Cela signifie qu'un contrat a été passé avec nous et que l'entreprise a été validée)
                        if (this.tenantSettings.Companies.ContainsKey(company.Company_Tenant_Description))
                        {
                            this.Response.Cookies.Append("tenant-code", company.Company_Tenant_Description);
                            /*var currentCompany = this.tenantService.GetTenant();
                            ViewBag.CurrentCompany = new TenantSiteModel
                            {
                                Key = this.tenantService.GetTenantCode(),
                                Logo = currentCompany?.logo,
                                Name = currentCompany?.name
                            };*/
                        }
                        return RedirectToAction("Login", "Connection", new { area = "Application" });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return View(companyConnectionViewModel);
        }
        #endregion

        #region AccountConnection
        //GET
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            //Clear the existing external cookie to ensure a clear login process
            /*await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);*/

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
                        return RedirectToAction("Index", "Home", new { area = "Application" });
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

        //POST
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("CompanyConnection", "Connection", new { area = "Application" });
        }

        //GET
        [HttpGet]
        public async Task<IActionResult> Signup(string returnUrl = null)
        {
            SignupViewModel signupViewModel = new SignupViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            Response.Cookies.Append("tenant-code", "pipldeveloppement-1");

            return View(signupViewModel);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel signupViewModel)
        {
            try
            {
                /*returnUrl ??= Url.Content("~/");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();*/
                if (ModelState.IsValid)
                {
                    if (signupViewModel.Password != signupViewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError(string.Empty, "Les mots de passes ne correspondent pas");
                        return View(signupViewModel);
                    }

                    var user = CreateUser();
                    //Je crée le nom d'utilisateur à partir du nom et du prénom de l'utilisateur
                    var username = signupViewModel.Lastname.Substring(0, Math.Min(signupViewModel.Lastname.Length, 3))
                        + signupViewModel.Firstname.Substring(0, Math.Min(signupViewModel.Firstname.Length, 3));
                    user.Firstname = signupViewModel.Firstname;
                    user.Lastname = signupViewModel.Lastname;
                    await _userStore.SetUserNameAsync(user, username, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, signupViewModel.Email, CancellationToken.None);


                    var result = await this.userManager.CreateAsync(user, signupViewModel.Password);

                    if (result.Succeeded)
                    {
                        //_logger.LogInformation("Vous avez créé un nouveau compte avec votre mot de passe");

                        var userId = await this.userManager.GetUserIdAsync(user);
                        /*await this.userManager.AddToRoleAsync(user, Roles.Basic.ToString());*/
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                        /*code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);*/

                        /*await _emailSender.SendEmailAsync(signupViewModel.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            //Tant que l'email n'aura pas été validé, l'utilisateur aura une notif de valider son adresse mail
                            return RedirectToAction("Index", "Home", new { area = "Application" });
                            /*return RedirectToPage("RegisterConfirmation", new { email = signupViewModel.Email, returnUrl = returnUrl });*/
                        }
                        else
                        {
                            await this.signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Home", new { area = "Application" });
                            /*return LocalRedirect(returnUrl);*/
                        }
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(signupViewModel);
            }
            catch (Exception ex)
            {
                return View(signupViewModel);
            }
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
        #endregion
    }
}
