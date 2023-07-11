using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Exceptions.Application.Department;
using SaaS.DataAccess.Exceptions.Application.Subsidiary;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.ViewModels.Application.Subsidiary;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class SubsidiaryController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<SubsidiaryViewModel> Subsidiaries = new List<SubsidiaryViewModel>();

        public SubsidiaryController(IApplicationUnitOfWork applicationUnitOfWork,
            TenantService tenantService,
            IOptions<TenantSettings> options)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            Subsidiary subsidiary = new Subsidiary();
            return View(subsidiary);
        }

        [HttpPost]
        public IActionResult Create(Subsidiary subsidiary)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    subsidiary.CreatedBy = "IPPOLITI Pierre-Louis";
                else
                    subsidiary.CreatedBy = User?.Identity?.Name;

                try
                {
                    IEnumerable<Subsidiary> subsidiaries = this.applicationUnitOfWork.Subsidiary.GetAll();
                    foreach (Subsidiary sub in subsidiaries)
                    {
                        if (sub.Name == subsidiary.Name)
                        {
                            throw new SubsidiaryNameAlreadyExistsException();
                        }
                        if (sub.Code == subsidiary.Code)
                        {
                            throw new SubsidiaryCodeAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.Subsidiary.Add(subsidiary);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"La filliale a bien été ajoutée à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création filliale";
                    TempData["success-message"] = "La filliale a bien été créée";
                    return RedirectToAction("Index");
                }
                catch (SubsidiaryNameAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Le nom de cette filliale existe déjà";
                    return View(subsidiary);
                }
                catch (SubsidiaryCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Le code de cette filliale existe déjà";
                    return View(subsidiary);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une filliale dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création filliale";
                    TempData["warning-message"] = "Erreur lors de la création d'une filliale";
                    return View(subsidiary);
                }
            }
            return View();
        }
    }
}
