using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Company;
using SaaS.Domain.Identity;
using SaaS.Domain.PIPL;
using SaaS.Domain.Tenant;
using SaaS.ViewModels.Application.Connection;
using SaaS.ViewModels.SuperCompany.Company;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class CompanyController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;
        private readonly IWebHostEnvironment hostingEnvironment;

        private static readonly ConfigurationCompanyViewModel configurationCompanyViewModel = new ConfigurationCompanyViewModel();
        public CompanyController(IApplicationUnitOfWork applicationUnitOfWork,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ISuperCompanyUnitOfWork superCompanyUnitOfWork, 
            TenantService tenantService,
            IOptions<TenantSettings> options,
            IUserStore<IdentityUser> userStore,
            IWebHostEnvironment webHostEnvironment)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.superCompanyUnitOfWork = superCompanyUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
            this.userStore = userStore;
            this.hostingEnvironment = webHostEnvironment;
            this.emailStore = GetEmailStore();
        }

        public IActionResult Index()
        {
            /*if (this.User?.Identity?.Name == "Pierre-Louis")
                return View();
            else
                return RedirectToAction("AccessDenied", "Connection", new { area = "Application" });*/
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateCompanyViewModel createCompanyViewModel = new CreateCompanyViewModel();
            return View(createCompanyViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateCompanyViewModel createCompanyViewModel/*, IFormFile picture*/)
        {
            /*Avant d'ajouter une nouvelle entreprise à la base, il faut vérifier que son nom, son SIRET, son CompanyCode 
             * et son Company_Tenant_Description ne soient pas déjà enregistrés dans la base de données.*/
            if (ModelState.IsValid)
            {
                try
                {
                    this.superCompanyUnitOfWork.Company.Add(createCompanyViewModel.Company);
                    this.superCompanyUnitOfWork.Save();
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une entreprise dans la base de données", "Exception", LogType.Error);
                    TempData["error-title"] = "Création d'entreprise";
                    TempData["error-message"] = "Une erreur est survenue lors de la création d'une entreprise en base de données";
                }

                string wwwRootPath = this.hostingEnvironment.WebRootPath;
                /*if (picture != null)
                {
                    try
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
                        string companyPath = @"images\companies\company-" + createCompanyViewModel.Company.Id;
                        string finalPath = Path.Combine(wwwRootPath, companyPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            picture.CopyTo(fileStream);
                        }

                        CompanyPicture companyPicture = new CompanyPicture()
                        {
                            ImageUrl = @"\" + companyPath + @"\" + fileName,
                            CompanyId = createCompanyViewModel.Company.Id,
                        };

                        //if (createCompanyViewModel.Company.Picture == null)
                            //createCompanyViewModel.Company.Picture = new CompanyPicture();

                        //createCompanyViewModel.Company.Picture = companyPicture;
                    }
                    catch (ArgumentException ex)
                    {
                        this.superCompanyUnitOfWork.Log.Add(new Domain.PIPL.Log
                        {
                            ExceptionName = "ArgumentException",
                            Message = ex.Message,
                            Source = ex.Source,
                            DevNote = "Erreur lors de l'ajout d'une photo d'entreprise dans le fichier wwwroot",
                            LogType = LogType.Error,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User?.Identity.Name,
                            IsEnable = true,
                        });
                        this.superCompanyUnitOfWork.Save();
                        TempData["error-title"] = "Création d'entreprise";
                        TempData["error-message"] = "Une erreur est survenue lors de l'ajout d'une photo d'entreprise dans le fichier wwwroot";
                    }
                    catch (Exception ex)
                    {
                        this.superCompanyUnitOfWork.Log.Add(new Domain.PIPL.Log
                        {
                            ExceptionName = "Exception",
                            Message = ex.Message,
                            Source = ex.Source,
                            DevNote = "Erreur lors de l'ajout d'une photo d'entreprise dans le fichier wwwroot",
                            LogType = LogType.Error,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User?.Identity.Name,
                            IsEnable = true,
                        });
                        this.superCompanyUnitOfWork.Save();
                        TempData["error-title"] = "Création d'entreprise";
                        TempData["error-message"] = "Une erreur est survenue lors de l'ajout d'une photo d'entreprise dans le fichier wwwroot";
                    }
                }*/

                try
                {
                    /*Création du tenant*/
                    this.tenantService.AddTenant(createCompanyViewModel.Company.Id, createCompanyViewModel.Company.Name,
                                                    "picture.FileName", createCompanyViewModel.ConnectionString);
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'un tenant dans le fichier appsettings", "Exception", LogType.Error);
                    TempData["error-title"] = "Création d'entreprise";
                    TempData["error-message"] = "Une erreur est survenue lors de l'ajout d'un tenant dans le fichier appsettings";
                }

                try
                {
                    this.superCompanyUnitOfWork.Company.Update(createCompanyViewModel.Company);
                    this.superCompanyUnitOfWork.Save();
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de la sauvegarde des modifications de l'entreprise dans la base de données", "Exception", LogType.Error);
                    TempData["error-title"] = "Création d'entreprise";
                    TempData["error-message"] = "Erreur lors de la sauvegarde des modifications de l'entreprise dans la base de données";
                }

                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise {createCompanyViewModel.Company.Name} a été créée avec succès", "", LogType.Success);
                TempData["success-title"] = "Création d'entreprise";
                TempData["success-message"] = $"L'entreprise {createCompanyViewModel.Company.Name} a été créée avec succès";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'id passé en paramètre est null ou vide", "", LogType.Warning);
                TempData["warning-title"] = "Modification d'entreprise";
                TempData["warning-message"] = $"L'id passé en paramètre est null ou vide";
                return NotFound();
            }
            
            //Modifier l'entreprise
            Company company = this.superCompanyUnitOfWork.Company.Get(c => c.Id == id);

            if(company is null)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise avec l'Id : {id} n'a pas été trouvée", "", LogType.Warning);
                TempData["warning-title"] = "Recherche d'entreprise";
                TempData["warning-message"] = $"L'entreprise avec pour Id : {id} est introuvable";

                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.superCompanyUnitOfWork.Company.Update(company);
                    this.superCompanyUnitOfWork.Save();
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la sauvegarde des modifications de l'entreprise {company.Name} dans la base de données", "Exception", LogType.Error);
                    TempData["error-title"] = "Modification entreprise";
                    TempData["error-message"] = $"Erreur lors de la sauvegarde des modifications de l'entreprise {company.Name} dans la base de données";
                }

                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise {company.Name} a été modifiée avec succès", "", LogType.Success);
                TempData["success-title"] = "Modification entreprise";
                TempData["success-message"] = $"L'entreprise {company.Name} a été modifiée avec succès";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Configuration()
        {
            try
            {
                var company = this.tenantService.GetTenant();
                var id = this.tenantService.GetTenantCode();
                if (company != null)
                {
                    ViewBag.SelectedCompany = new TenantSiteModel
                    {
                        Key = this.tenantService.GetTenantCode(),
                        Logo = company.logo,
                        Name = company.name
                    };
                }

                if (id == null || string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }

                IEnumerable<Functionnality> functionnalities = this.superCompanyUnitOfWork.Functionnality.GetAll();

                //Modifier l'entreprise
                configurationCompanyViewModel.Company = this.superCompanyUnitOfWork.Company.Get(c => c.Id == id);

                configurationCompanyViewModel.HaveFunctionnalities.Clear();
                configurationCompanyViewModel.DontHaveFunctionnalities.Clear();

                if (configurationCompanyViewModel.Company is not null)
                {
                    foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                    {
                        if (tenantData.name == configurationCompanyViewModel.Company.Name)
                        {
                            using (ApplicationDbContext context = new ApplicationDbContext(tenantData.connectionString))
                            {
                                IEnumerable<CompanyFunctionnalities> companyFunctionnalities = context.CompanyFunctionnalities.ToList();

                                if (companyFunctionnalities.Count() == 0)
                                {
                                    foreach (Functionnality functionnality in functionnalities)
                                    {
                                        configurationCompanyViewModel.DontHaveFunctionnalities.Add(new CompanyFunctionnalities
                                        {
                                            Code = functionnality.Code,
                                            CreatedBy = functionnality.CreatedBy,
                                            CreatedOn = functionnality.CreatedOn,
                                            Description = functionnality.Description,
                                            IsEnable = functionnality.IsEnable,
                                            Name = functionnality.Name,
                                            NormalizedName = functionnality.NormalizedName,
                                        });
                                    }
                                }
                                else
                                {
                                    foreach (Functionnality functionnality in functionnalities)
                                    {
                                        if (companyFunctionnalities.Any(cf => cf.Name == functionnality.Name))
                                        {
                                            configurationCompanyViewModel.HaveFunctionnalities.Add(new CompanyFunctionnalities
                                            {
                                                Code = functionnality.Code,
                                                CreatedBy = functionnality.CreatedBy,
                                                CreatedOn = functionnality.CreatedOn,
                                                Description = functionnality.Description,
                                                IsEnable = functionnality.IsEnable,
                                                Name = functionnality.Name,
                                                NormalizedName = functionnality.NormalizedName,
                                            });
                                        }
                                        else
                                        {
                                            configurationCompanyViewModel.DontHaveFunctionnalities.Add(new CompanyFunctionnalities
                                            {
                                                Code = functionnality.Code,
                                                CreatedBy = functionnality.CreatedBy,
                                                CreatedOn = functionnality.CreatedOn,
                                                Description = functionnality.Description,
                                                IsEnable = functionnality.IsEnable,
                                                Name = functionnality.Name,
                                                NormalizedName = functionnality.NormalizedName,
                                            });
                                        }
                                    }
                                }

                                return View(configurationCompanyViewModel);
                            }
                        }
                    }

                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise avec l'Id : {id} n'a pas été trouvée", "", LogType.Warning);
                    TempData["warning-title"] = "Configuration entreprise";
                    TempData["warning-message"] = $"L'entreprise avec pour Id : {id} est introuvable";
                    return NotFound();
                }
            }
            catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise avec l'Id :  n'a pas été trouvée", "", LogType.Error);
                    TempData["error-title"] = "Configuration entreprise";
                    TempData["error-message"] = $"L'entreprise avec pour Id :  est introuvable";
            }
            return View(configurationCompanyViewModel);
        }

        [HttpGet]
        public IActionResult CreateCompanyUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyUser(SignupViewModel signupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (signupViewModel.Password != signupViewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError(string.Empty, "Les mots de passes ne correspondent pas");
                        TempData["error-title"] = "Création utilisateur entreprise";
                        TempData["error-message"] = "Les mots de passes de correspondent pas";
                        return View(signupViewModel);
                    }

                    var user = CreateUser();
                    //Je crée le nom d'utilisateur à partir du nom et du prénom de l'utilisateur
                    var username = signupViewModel.User.Lastname.Substring(0, Math.Min(signupViewModel.User.Lastname.Length, 3))
                        + signupViewModel.User.Firstname.Substring(0, Math.Min(signupViewModel.User.Firstname.Length, 3));
                    user.Firstname = signupViewModel.User.Firstname;
                    user.Lastname = signupViewModel.User.Lastname;
                    await userStore.SetUserNameAsync(user, username, CancellationToken.None);
                    await emailStore.SetEmailAsync(user, signupViewModel.User.Email, CancellationToken.None);

                    var result = await this.userManager.CreateAsync(user, signupViewModel.Password);

                    if (result.Succeeded)
                    {
                        this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'utilisateur {username} a été ajouté à l'entreprise", "", LogType.Success);
                        TempData["success-title"] = "Création utilisateur entreprise";
                        TempData["success-message"] = $"L'utilisateur a été ajouté à l'entreprise";
                        return RedirectToAction("Configuration", "Company", new { area = "SuperCompany" });
                    }
                    foreach (var error in result.Errors)
                    {
                        this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"{error}", "", LogType.Error);
                        TempData["error-title"] = "Création utilisateur entreprise";
                        TempData["error-message"] = $"{error}";
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(signupViewModel);
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"", "", LogType.Error);
                TempData["error-title"] = "Création utilisateur entreprise";
                TempData["error-message"] = $"{ex.Message}";
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

        #region API CALLS
        [HttpGet]
        [OutputCache(Duration = 0)]
        public IActionResult GetAllCompanies()
        {
            /*Ecrire la requête en SQL pour récupérer les entreprises si nécessaire*/
            IEnumerable<Company> companies = this.superCompanyUnitOfWork.Company.GetAll().Where(c => c.IsSuperCompany == false);

            return Json(new { data = companies });
        }

        /// <summary>
        /// Retrieve all the application's functionalities and see if the company has them. If so, returns true, otherwise false.
        /// </summary>
        /// <param name="id">Id of the company</param>
        /// <returns>JSON</returns>
        [HttpGet]
        public IActionResult GetAllFunctionnalitiesForCompanyConfiguration([FromBody]string id)
        {
            /*Décomposition en trois étapes : 
                1. Récupérer l'ensemble des fonctionnalités présentes dans l'application
                2. Pour chacune de ces fonctionnalités, vérifier si l'entreprise (id en paramètre) possède cette fonctionnalité
                3. Si c'est le cas, ajouter une colonne correspondant à un booléen
            */

            IEnumerable<Functionnality> functionnalities = this.superCompanyUnitOfWork.Functionnality.GetAll().Where(c => c.IsEnable == true);

            IList<CompanyFunctionnalities> tests = new List<CompanyFunctionnalities>();
            foreach (Functionnality functionnality in functionnalities)
            {
                tests.Add(new CompanyFunctionnalities
                {
                    Id = functionnality.Id,
                    Name = functionnality.Name, 
                    Description = functionnality.Description,
                    /*HasAccess = this.superCompanyUnitOfWork.Company.HasFunctionnality(this.superCompanyUnitOfWork.Company.GetAll().FirstOrDefault(c => c.Id == id), functionnality)*/
                });
            }

            return Json(new { data = tests });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            /*Logique pour la suppression d'une entreprise*/
            Company company = this.superCompanyUnitOfWork.Company.Get(c => c.Id == id);
            try
            {
                this.superCompanyUnitOfWork.Company.Delete(company);
                this.superCompanyUnitOfWork.Save();
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise {company.Name} a été supprimée de la base de données", "", LogType.Success);
                TempData["success-title"] = "Suppression entreprise";
                TempData["success-message"] = $"L'entreprise {company.Name} a été supprimée de la base de données";
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de l'entreprise {company.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Suppression entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression de l'entreprise {company.Name} dans la base de données";
            }

            //Logique pour la suppression d'une image d'entreprise
            /*CompanyPicture companyPicture = this.applicationUnitOfWork.CompanyPicture.Get(cp => cp.CompanyId == id);
            try
            {
                this.applicationUnitOfWork.CompanyPicture.Delete(companyPicture);
                this.applicationUnitOfWork.Save();
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'image de l'entreprise {company.Name} a été supprimée", "", LogType.Success);
                TempData["success-title"] = "Suppression image entreprise";
                TempData["success-message"] = $"L'image de l'entreprise {company.Name} a été supprimée";
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de l'icône de l'entreprise {company.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Suppression icône entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression de l'icône de l'entreprise {company.Name}";
            }*/

            try
            {
                this.tenantService.DeleteTenant(company.Id);
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le tenant de l'entreprise {company.Name} a été supprimé", "", LogType.Success);
                TempData["success-title"] = "Suppression tenant entreprise";
                TempData["success-message"] = $"Le tenant de l'entreprise {company.Name} a été supprimé";
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression du tenant de l'entreprise {company.Name} dans le fichier appsettings", "Exception", LogType.Error);
                TempData["error-title"] = "Suppression tenant entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression du tenant de l'entreprise {company.Name} dans le fichier appsettings";
            }

            try
            {
                string wwwRootPath = this.hostingEnvironment.WebRootPath;
                string companyPath = @"images\companies\company-" + id;
                string finalPath = Path.Combine(wwwRootPath, companyPath);
                if (Directory.Exists(finalPath))
                {
                    Directory.Delete(finalPath, true);
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'image de l'entreprise {company.Name} a été supprimé dans les fichiers de l'application", "", LogType.Success);
                    TempData["success-title"] = "Suppression image entreprise";
                    TempData["success-message"] = $"L'image de l'entreprise {company.Name} a été supprimé dans les fichiers de l'application";
                }
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de l'image de l'entreprise {company.Name} dans le dossier de l'application", "Exception", LogType.Error);
                TempData["error-title"] = "Suppression tenant entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression du tenant de l'entreprise {company.Name} dans le fichier appsettings";
            }

            return View();
        }

        [HttpGet]
        public IActionResult GoCompany(string? id)
        {
            if (id is not null)
            {
                if (this.tenantSettings.Companies.ContainsKey(id))
                {
                    Response.Cookies.Append("tenant-code", id);
                }
            }
            return RedirectToAction("Configuration");
        }

        [HttpPost]
        public IActionResult ExitCompany()
        {
            Response.Cookies.Delete("tenant-code");
            Company company = this.superCompanyUnitOfWork.Company.Get(c => c.IsSuperCompany == true);
            if (company is not null)
            {
                if (this.tenantSettings.Companies.ContainsKey(company.Id))
                {
                    Response.Cookies.Append("tenant-code", company.Id);
                }
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Logique de modification d'état des entreprises.
        /// Il est possible d'activer ou de desactiver une entreprise. Lorsqu'elle est desactivée, aucun utilisateur de cette 
        /// entreprise ne peut y accéder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LockUnlockCompany([FromBody]string id)
        {
            var objFromDb = new Company();
            try
            {
                objFromDb = this.superCompanyUnitOfWork.Company.Get(c => c.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de l'entreprise" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.superCompanyUnitOfWork.Company.Update(objFromDb);
                this.superCompanyUnitOfWork.Save();
                if(objFromDb.IsEnable == true)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise {objFromDb.Name} a bien été activée", "", LogType.Success);
                    TempData["success-title"] = "Activation entreprise";
                    TempData["success-message"] = $"L'entreprise {objFromDb.Name} a bien été activée";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"L'entreprise {objFromDb.Name} a bien été desactivée", "", LogType.Success);
                    TempData["success-title"] = "Desactivation entreprise";
                    TempData["success-message"] = $"L'entreprise {objFromDb.Name} a bien été desactivée";
                }
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état de l'entreprise {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état entreprise";
                TempData["error-message"] = $"Erreur lors de la modification de l'état de l'entreprise {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation de l'entreprise réussie" });
        }

        [HttpPost]
        public IActionResult AddFunctionnalityToCompany(string functionnalityName)
        {
            Functionnality functionnality = this.superCompanyUnitOfWork.Functionnality.Get(f => f.Name == functionnalityName);

            try
            {
                if (this.tenantSettings.Companies.ContainsKey(configurationCompanyViewModel.Company.Id))
                {
                    foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                    {
                        if (tenantData.name == configurationCompanyViewModel.Company.Name)
                        {
                            using (ApplicationDbContext context = new ApplicationDbContext(tenantData.connectionString))
                            {
                                context.CompanyFunctionnalities.Add(new CompanyFunctionnalities
                                {
                                    Code = functionnality.Code,
                                    Description = functionnality.Description,
                                    CreatedBy = functionnality.CreatedBy,
                                    CreatedOn = functionnality.CreatedOn,
                                    IsEnable = functionnality.IsEnable,
                                    Name = functionnality.Name,
                                });
                                context.SaveChanges();
                                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité a bien été ajoutée", "", LogType.Success);
                                TempData["success-title"] = "Ajout fonctionnalité entreprise";
                                TempData["success-message"] = $"La fonctionnalité a bien été ajoutée";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de l'ajout de la fonctionnalité {functionnality.Name} à l'entreprise dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Ajout fonctionnalité entreprise";
                TempData["error-message"] = $"Erreur lors de l'ajout de la fonctionnalité {functionnality.Name} à l'entreprise dans la base de données";
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeleteFunctionnalityToCompany(string functionnalityName)
        {
            Functionnality functionnality = this.superCompanyUnitOfWork.Functionnality.Get(f => f.Name == functionnalityName);

            try
            {
                if (this.tenantSettings.Companies.ContainsKey(configurationCompanyViewModel.Company.Id))
                {
                    foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                    {
                        if (tenantData.name == configurationCompanyViewModel.Company.Name)
                        {
                            using (ApplicationDbContext context = new ApplicationDbContext(tenantData.connectionString))
                            {
                                CompanyFunctionnalities companyFunctionnalities = this.applicationUnitOfWork.CompanyFunctionnalities.Get(cf => cf.Name == functionnality.Name);

                                context.CompanyFunctionnalities.Remove(companyFunctionnalities);
                                context.SaveChanges();
                                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité a bien été supprimée", "", LogType.Success);
                                TempData["success-title"] = "Suppression fonctionnalité entreprise";
                                TempData["success-message"] = $"La fonctionnalité a bien été supprimée";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de la fonctionnalité {functionnality.Name} à l'entreprise dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Ajout fonctionnalité entreprise";
                TempData["error-message"] = $"Erreur lors de la suppression de la fonctionnalité {functionnality.Name} à l'entreprise dans la base de données";
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetCompanyUsers()
        {
            if (this.tenantSettings.Companies is not null)
            {
                foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                {
                    if (tenantData.name == configurationCompanyViewModel.Company.Name)
                    {
                        using (SqlConnection connection = new SqlConnection(tenantData.connectionString))
                        {
                            try
                            {
                                connection.Open();
                                var query = "SELECT AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.IsEnable FROM AspNetUsers;";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    configurationCompanyViewModel.CompanyUsers.Add(new Domain.Identity.User
                                    {
                                        Id = reader.GetString(0),
                                        UserName = reader.GetString(1),
                                        IsEnable = reader.GetBoolean(2),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = configurationCompanyViewModel.CompanyUsers });
                            }
                            catch (Exception ex)
                            {
                                /*this.superCompanyLogService.CreateNewEventInlog(ex, User, $"Erreur lors de la suppression de la fonctionnalité {.Name} à l'entreprise dans la base de données", "Exception", LogType.Error);
                                TempData["error-title"] = "Ajout fonctionnalité entreprise";
                                TempData["error-message"] = $"Erreur lors de la suppression de la fonctionnalité {functionnality.Name} à l'entreprise dans la base de données";*/
                                return Json(null);
                            }
                        }
                    }
                }
                return Json(null);
            }
            return View();
        }
        #endregion

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)userStore;
        }
    }
}
