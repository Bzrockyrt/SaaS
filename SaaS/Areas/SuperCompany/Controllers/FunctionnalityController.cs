using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Exceptions.SuperCompany.Functionnality;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain;
using SaaS.Domain.PIPL;
using SaaS.ViewModels.SuperCompany.Functionnality;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class FunctionnalityController : Controller
    {
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;

        public static DetailsFunctionnalityViewModel detailsFunctionnalityViewModel = new DetailsFunctionnalityViewModel();
        public FunctionnalityController(ISuperCompanyUnitOfWork superCompanyUnitOfWork)
        {
            this.superCompanyUnitOfWork = superCompanyUnitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateFunctionnalityViewModel createFunctionnalityViewModel = new CreateFunctionnalityViewModel()
            {
                Functionnality = new Functionnality()
            };
            return View(createFunctionnalityViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateFunctionnalityViewModel createFunctionnalityViewModel)
        {
            /*Avant d'ajouter une nouvelle entreprise à la base, il faut vérifier que son nom, son SIRET, son CompanyCode 
             * et son Company_Tenant_Description ne soient pas déjà enregistrés dans la base de données.*/
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    createFunctionnalityViewModel.Functionnality.CreatorId = string.Empty;
                else
                    createFunctionnalityViewModel.Functionnality.CreatorId = this.superCompanyUnitOfWork.User.GetAll().FirstOrDefault(u => u.UserName == User?.Identity?.Name).Id;
                try
                {
                    IEnumerable<Functionnality> functs = this.superCompanyUnitOfWork.Functionnality.GetAll();
                    foreach (Functionnality func in functs)
                    {
                        if (func.Name == createFunctionnalityViewModel.Functionnality.Name)
                        {
                            throw new FunctionnalityNameAlreadyExistsException();
                        }
                        if (func.Code == createFunctionnalityViewModel.Functionnality.Code)
                        {
                            throw new FunctionnalityCodeAlreadyExistsException();
                        }
                    }

                    this.superCompanyUnitOfWork.Functionnality.Add(createFunctionnalityViewModel.Functionnality);
                    this.superCompanyUnitOfWork.Save();
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité a bien été ajoutée à la base de données", "", LogType.Success);
                    TempData["success-title"] = "Création fonctionnalité";
                    TempData["success-message"] = "La fonctionnalité a bien été créée";
                    return RedirectToAction("Index");
                }
                catch (FunctionnalityNameAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création fonctionnalité";
                    TempData["warning-message"] = "Le nom de cette fonctionnalité existe déjà";
                    return View(createFunctionnalityViewModel);
                }
                catch (FunctionnalityCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création fonctionnalité";
                    TempData["warning-message"] = "Le code de cette fonctionnalité existe déjà";
                    return View(createFunctionnalityViewModel);
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une fonctionnalité dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création fonctionnalité";
                    TempData["warning-message"] = "Erreur lors de la création d'une fonctionnalité";
                    return View(createFunctionnalityViewModel);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            detailsFunctionnalityViewModel.Functionnality = this.superCompanyUnitOfWork.Functionnality.Get(s => s.Id == id);

            if (detailsFunctionnalityViewModel.Functionnality is null)
            {
                return NotFound();
            }

            return View(detailsFunctionnalityViewModel);
        }

        [HttpPost]
        public IActionResult Details(DetailsFunctionnalityViewModel detailsFunctionnalityViewModel)
        {
            if (ModelState.IsValid)
            {
                detailsFunctionnalityViewModel.Functionnality.CreatorId = "";
                detailsFunctionnalityViewModel.Functionnality.UpdatedOn = DateTime.Now;
                detailsFunctionnalityViewModel.Functionnality.UpdatedBy = User?.Identity.Name;
                this.superCompanyUnitOfWork.Functionnality.Update(detailsFunctionnalityViewModel.Functionnality);
                this.superCompanyUnitOfWork.Save();

                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"Le poste a bien été modifié", "", LogType.Success);
                TempData["success-title"] = "Modification poste";
                TempData["success-message"] = $"Le poste a bien été modifié";
                return RedirectToAction("Index");
            }
            return View(detailsFunctionnalityViewModel);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllFunctionnalities()
        {
            /*Ecrire la requête en SQL pour récupérer les fonctionnalités si nécessaire*/
            IEnumerable<Functionnality> functionnalities = this.superCompanyUnitOfWork.Functionnality.GetAll();

            return Json(new { data = functionnalities });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            /*Logique pour la suppression d'une fonctionnalité*/
            return View();
        }

        [HttpPost]
        public IActionResult LockUnlockFunctionnality([FromBody]string id)
        {
            var objFromDb = new Functionnality();
            try
            {
                objFromDb = this.superCompanyUnitOfWork.Functionnality.Get(f => f.Id == id);
                if (objFromDb is null)
                    return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de la fonctionnalité" });
                if (objFromDb.IsEnable == false)
                    objFromDb.IsEnable = true;
                else
                    objFromDb.IsEnable = false;

                this.superCompanyUnitOfWork.Functionnality.Update(objFromDb);
                this.superCompanyUnitOfWork.Save();
                if (objFromDb.IsEnable == true)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité {objFromDb.Name} a bien été activée", "", LogType.Success);
                    TempData["success-title"] = "Activation fonctionnalité";
                    TempData["success-message"] = $"La fonctionnalité {objFromDb.Name} a bien été activée";
                }
                else if (objFromDb.IsEnable == false)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité {objFromDb.Name} a bien été desactivée", "", LogType.Success);
                    TempData["success-title"] = "Desactivation fonctionnalité";
                    TempData["success-message"] = $"La fonctionnalité {objFromDb.Name} a bien été desactivée";
                }
            }
            catch (Exception ex)
            {
                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, $"Erreur lors de la modification de l'état de la fonctionnalité {objFromDb.Name} dans la base de données", "Exception", LogType.Error);
                TempData["error-title"] = "Modification état fonctionnalité";
                TempData["error-message"] = $"Erreur lors de la modification de l'état de la fonctionnalité {objFromDb.Name} dans la base de données";
            }

            return Json(new { success = true, message = "Activation/désactivation de la fonctionnalité réussie" });
        }
        #endregion
    }
}
