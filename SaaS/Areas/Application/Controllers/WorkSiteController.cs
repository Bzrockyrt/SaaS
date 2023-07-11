using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Exceptions.Application.WorkSite;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain;
using SaaS.Domain.PIPL;
using SaaS.Domain.Work;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class WorkSiteController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;

        public WorkSiteController(IApplicationUnitOfWork applicationUnitOfWork)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            WorkSite workSite = new WorkSite();
            return View(workSite);
        }

        [HttpPost]
        public IActionResult Create(WorkSite workSite)
        {
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    workSite.CreatedBy = "IPPOLITI Pierre-Louis";
                else
                    workSite.CreatedBy = User?.Identity?.Name;
                try
                {
                    IEnumerable<WorkSite> workSites = this.applicationUnitOfWork.WorkSite.GetAll();
                    foreach (WorkSite wSite in workSites)
                    {
                        if (wSite.Name == workSite.Name)
                        {
                            throw new WorkSiteNameAlreadyExistsException();
                        }
                    }

                    this.applicationUnitOfWork.WorkSite.Add(workSite);
                    this.applicationUnitOfWork.Save();
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier a bien été ajoutée à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création chantier";
                    TempData["success-message"] = "Le chantier a bien été créée";
                    return RedirectToAction("Index");
                }
                catch (WorkSiteNameAlreadyExistsException ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une chantier dans la base de données", "WorkSiteNameAlreadyExistsException", LogType.Warning);
                    TempData["warning-title"] = "Création chantier";
                    TempData["warning-message"] = "Le nom de ce chantier existe déjà";
                    return View(workSite);
                }
                catch (Exception ex)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'un chantier dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création chantier";
                    TempData["warning-message"] = "Erreur lors de la création d'un chantier";
                    return View(workSite);
                }
            }
            return View();
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAllWorkSites()
        {
            /*Ecrire la requête en SQL pour récupérer les chantiers si nécessaire*/
            IEnumerable<WorkSite> workSites = this.applicationUnitOfWork.WorkSite.GetAll();

            return Json(new { data = workSites });
        }

        [HttpPost]
        public IActionResult LockUnlockWorkSite([FromBody] string id)
        {
            var objFromDb = new WorkSite();
            try
            {
                objFromDb = this.applicationUnitOfWork.WorkSite.Get(f => f.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation du chantier" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.applicationUnitOfWork.WorkSite.Update(objFromDb);
                this.applicationUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier {objFromDb.Name} a bien été activé", "", LogType.Success);
                    TempData["success-title"] = "Activation chantier";
                    TempData["success-message"] = $"Le chantier {objFromDb.Name} a bien été activé";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.applicationUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le chantier {objFromDb.Name} a bien été desactivé", "", LogType.Success);
                    TempData["success-title"] = "Désactivation chantier";
                    TempData["success-message"] = $"Le chantier {objFromDb.Name} a bien été desactivé";
                }
            }
            catch (Exception ex)
            {
                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état du chantier {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état chantier";
                TempData["error-message"] = $"Erreur lors de la modification du chantier {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation du chantier réussie" });
        }
        #endregion
    }
}
