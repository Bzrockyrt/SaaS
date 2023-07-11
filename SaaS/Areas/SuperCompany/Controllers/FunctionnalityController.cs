using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Exceptions.SuperCompany.Functionnality;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.DataAccess.Services;
using SaaS.Domain;
using SaaS.Domain.PIPL;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class FunctionnalityController : Controller
    {
        private readonly ISuperCompanyUnitOfWork superCompanyUnitOfWork;

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
            Functionnality functionnality = new Functionnality();
            return View(functionnality);
        }

        [HttpPost]
        public IActionResult Create(Functionnality functionnality)
        {
            /*Avant d'ajouter une nouvelle entreprise à la base, il faut vérifier que son nom, son SIRET, son CompanyCode 
             * et son Company_Tenant_Description ne soient pas déjà enregistrés dans la base de données.*/
            if (ModelState.IsValid)
            {
                if (User?.Identity?.Name is null)
                    functionnality.CreatedBy = "IPPOLITI Pierre-Louis";
                else
                    functionnality.CreatedBy = User?.Identity?.Name;
                try
                {
                    IEnumerable<Functionnality> functs = this.superCompanyUnitOfWork.Functionnality.GetAll();
                    foreach (Functionnality func in functs)
                    {
                        if (func.Name == functionnality.Name)
                        {
                            throw new FunctionnalityNameAlreadyExistsException();
                        }
                        if (func.Code == functionnality.Code)
                        {
                            throw new FunctionnalityCodeAlreadyExistsException();
                        }
                    }

                    this.superCompanyUnitOfWork.Functionnality.Add(functionnality);
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
                    return View(functionnality);
                }
                catch (FunctionnalityCodeAlreadyExistsException ex)
                {
                    TempData["warning-title"] = "Création fonctionnalité";
                    TempData["warning-message"] = "Le code de cette fonctionnalité existe déjà";
                    return View(functionnality);
                }
                catch (Exception ex)
                {
                    this.superCompanyUnitOfWork.Log.CreateNewEventInlog(ex, User, "Erreur lors de l'ajout d'une fonctionnalité dans la base de données", "Exception", LogType.Error);
                    TempData["warning-title"] = "Création fonctionnalité";
                    TempData["warning-message"] = "Erreur lors de la création d'une fonctionnalité";
                    return View(functionnality);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            //Modifier l'entreprise
            Functionnality functionnality = this.superCompanyUnitOfWork.Functionnality.Get(c => c.Id == id);

            if (functionnality is null)
            {
                return NotFound();
            }
            return View(functionnality);
        }

        [HttpPost]
        public IActionResult Edit(Functionnality functionnality)
        {
            if (ModelState.IsValid)
            {
                this.superCompanyUnitOfWork.Functionnality.Update(functionnality);
                this.superCompanyUnitOfWork.Save();

                this.superCompanyUnitOfWork.Log.CreateNewEventInlog(null, User, $"La fonctionnalité a bien été modifiée", "", LogType.Success);
                TempData["success-title"] = "Modification fonctionnalité";
                TempData["success-message"] = $"La fonctionnalité a bien été modifiée";
                return RedirectToAction("Index");
            }
            return View(functionnality);
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
