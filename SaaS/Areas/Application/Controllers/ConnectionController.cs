using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain.Tenant;
using SaaS.ViewModels.Application.Connection;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class ConnectionController : Controller
    {
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;
        private readonly UserManager<IdentityUser> userManager;

        public ConnectionController(ISuperCompanyUnitOfWork superCompanyUnitOfWork,
            IApplicationUnitOfWork applicationUnitOfWork,
            SignInManager<IdentityUser> signInManager,
            TenantService tenantService,
            IOptions<TenantSettings> options,
            IUserStore<IdentityUser> userStore,
            UserManager<IdentityUser> userManager)
        {
            this.superCompanyUnitOfWork = superCompanyUnitOfWork;
            this.applicationUnitOfWork = applicationUnitOfWork;
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

        [HttpGet]
        public IActionResult AccessDenied()
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
                    var company = this.superCompanyUnitOfWork.Company.Get(c => c.CompanyCode == companyConnectionViewModel.CompanyCode);
                    if (company is null)
                    {
                        //Si 'company' est null, l'entreprise n'existe pas
                        ModelState.AddModelError(string.Empty, "L'entreprise n'existe pas");
                        return View();
                    }
                    else
                    {
                        //Si 'company' n'est pas null, je vérifie qu'elle possède bien une chaîne de connexion
                        //(Cela signifie qu'un contrat a été passé avec nous et que l'entreprise a été validée)
                        if (this.tenantSettings.Companies.ContainsKey(company.Id))
                        {
                            this.Response.Cookies.Append("tenant-code", company.Id);
                            ViewData["CurrentCompany"] = new TenantSiteModel()
                            {
                                Key = this.tenantService.GetTenantCode(),
                                Logo = this.tenantService.GetTenant()?.logo,
                                Name = this.tenantService.GetTenant()?.name,
                            };
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

            if (string.IsNullOrEmpty(this.tenantService.GetTenantName()))
            {
                //Le nom du tenant est null ou vide, c'est que je ne me suis pas connecté à l'entreprise
                return RedirectToAction("companyconnection", "connection", new { area = "application" });
            }
            else
            {
                LoginViewModel model = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins =
                    (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };

                return View(model);
            }
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.tenantService.GetTenantName() == "PIPL Développement")
                    {
                        Domain.PIPL.User us = this.superCompanyUnitOfWork.User.Get(u => u.Email == loginViewModel.Email);

                        if (us == null)
                        {
                            ModelState.AddModelError(string.Empty, "Utilisateur inexistant");
                            return View(loginViewModel);
                        }
                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var res = await signInManager.PasswordSignInAsync(us, loginViewModel.Password, loginViewModel.RememberMe, false);

                        if (res.Succeeded)
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "SuperCompany" });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Informations invalides");
                        }
                    }
                    else
                    {
                        /*var user = await userManager.FindByEmailAsync(loginViewModel.Email);*/

                        Domain.Identity.User user = this.applicationUnitOfWork.User.Get(u => u.Email == loginViewModel.Email);

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
            /*Lors de la déconnexion, il faut déconnecter le compte utilisateur mais également le compte de l'entreprise*/
            Response.Cookies.Delete("tenant-code");
            await this.signInManager.SignOutAsync();
            return View("CompanyConnection");
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

        private Domain.Identity.User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Domain.Identity.User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        #endregion
    }
}
