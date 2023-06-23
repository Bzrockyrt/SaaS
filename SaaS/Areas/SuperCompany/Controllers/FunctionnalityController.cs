using Microsoft.AspNetCore.Mvc;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;
using SaaS.Domain.PIPL;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class FunctionnalityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public FunctionnalityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
                this.unitOfWork.Functionnality.Add(functionnality);
                this.unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                //Redirection vers la création d'une fonctionnalité
                /*return View("Create");*/

                //ou return la page d'information NotFound
                return NotFound();
            }

            //Modifier l'entreprise
            Functionnality functionnality = this.unitOfWork.Functionnality.Get(c => c.Id == id);

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
                this.unitOfWork.Functionnality.Update(functionnality);
                this.unitOfWork.Save();

                TempData["success"] = "La fonctionnalité a bien été modifiée";
                return RedirectToAction("Index");
            }
            return View(functionnality);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllFunctionnalities()
        {
            /*Ecrire la requête en SQL pour récupérer les fonctionnalités si nécessaire*/
            IEnumerable<Functionnality> functionnalities = this.unitOfWork.Functionnality.GetAll();

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
            var objFromDb = this.unitOfWork.Functionnality.Get(f => f.Id == id);
            if(objFromDb is null)
                return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de la fonctionnalité" });
            if (objFromDb.IsEnable == false)
                objFromDb.IsEnable = true;
            else
                objFromDb.IsEnable = false;

            this.unitOfWork.Functionnality.Update(objFromDb);
            this.unitOfWork.Save();

            return Json(new { success = true, message = "Activation/désactivation de la fonctionnalité réussie" });
        }
        #endregion
    }
}
