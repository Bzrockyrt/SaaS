using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Data.SqlClient;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.Domain.Models;
using SaaS.Domain.OTHER;
using SaaS.Domain.PIPL;
using SaaS.ViewModels.SuperCompany.Company;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly TenantService tenantService;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CompanyController(IUnitOfWork unitOfWork, TenantService tenantService, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.tenantService = tenantService;
            this.hostingEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateCompanyViewModel createCompanyViewModel = new CreateCompanyViewModel();
            return View(createCompanyViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateCompanyViewModel createCompanyViewModel, IFormFile picture)
        {
            /*Avant d'ajouter une nouvelle entreprise à la base, il faut vérifier que son nom, son SIRET, son CompanyCode 
             * et son Company_Tenant_Description ne soient pas déjà enregistrés dans la base de données.*/
            if (ModelState.IsValid)
            {
                try
                {
                    this.unitOfWork.Company.Add(createCompanyViewModel.Company);
                    this.unitOfWork.Save();

                    string wwwRootPath = this.hostingEnvironment.WebRootPath;
                    if (picture != null)
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

                        /*if (createCompanyViewModel.Company.Picture == null)
                            createCompanyViewModel.Company.Picture = new CompanyPicture();

                        createCompanyViewModel.Company.Picture = companyPicture;*/
                    }

                    /*Création du tenant*/
                    this.tenantService.AddTenant(createCompanyViewModel.Company.Id, createCompanyViewModel.Company.Name,
                                                    picture.FileName, createCompanyViewModel.ConnectionString);


                    this.unitOfWork.Company.Update(createCompanyViewModel.Company);
                    this.unitOfWork.Save();
                    TempData["success-title"] = "Création d'entreprise";
                    TempData["success-message"] = $"L'entreprise {createCompanyViewModel.Company.Name} a été créée avec succès";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error-title"] = "Création d'entreprise";
                    TempData["error-message"] = "Une erreur est survenue lors de la création d'une entreprise";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                //Redirection vers la création d'une entreprise
                /*return View("Create");*/

                //ou return la page d'information NotFound
                return NotFound();
            }
            
            //Modifier l'entreprise
            Company company = this.unitOfWork.Company.Get(c => c.Id == id);

            if(company is null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.Company.Update(company);
                this.unitOfWork.Save();

                TempData["success"] = "L'entreprise a bien été modifiée";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Configuration(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                //Redirection vers la création d'une entreprise
                /*return View("Create");*/

                //ou return la page d'information NotFound
                return NotFound();
            }

            //Modifier l'entreprise
            Company company = this.unitOfWork.Company.Get(c => c.Id == id);

            if (company is null)
            {
                return NotFound();
            }
            return View(company);
        }

        /// <summary>
        /// Lorsque j'ajoute supprime une ou plusieurs fonctionnalités d'une entreprise, ces modifications doivent également
        /// apparaitre dans la base de données de l'entreprise. Je dois faire en sorte de récupérer la chaîne de connexion de
        /// l'entreprise en question pour aller modifier les données de la table fonctionnalités de l'entreprise
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Functionnalities(string? id)
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        [OutputCache(Duration = 0)]
        public IActionResult GetAllCompanies()
        {
            /*Ecrire la requête en SQL pour récupérer les entreprises si nécessaire*/
            IEnumerable<Company> companies = this.unitOfWork.Company.GetAll().Where(c => c.IsSuperCompany == false);

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

            IEnumerable<Functionnality> functionnalities = this.unitOfWork.Functionnality.GetAll().Where(c => c.IsEnable == true);

            IList<CompanyFunctionnalities> tests = new List<CompanyFunctionnalities>();
            foreach (Functionnality functionnality in functionnalities)
            {
                tests.Add(new CompanyFunctionnalities
                {
                    Id = functionnality.Id,
                    Name = functionnality.Name, 
                    Description = functionnality.Description,
                    HasAccess = this.unitOfWork.Company.HasFunctionnality(this.unitOfWork.Company.GetAll().FirstOrDefault(c => c.Id == id), functionnality)
                });
            }

            return Json(new { data = tests });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            /*Logique pour la suppression d'une entreprise*/
            Company company = this.unitOfWork.Company.Get(c => c.Id == id);
            this.unitOfWork.Company.Delete(company);
            this.unitOfWork.Save();

            /*Logique pour la suppression d'une image d'entreprise*/
            CompanyPicture companyPicture = this.unitOfWork.CompanyPicture.Get(cp => cp.CompanyId == id);
            this.unitOfWork.CompanyPicture.Delete(companyPicture);
            this.tenantService.DeleteTenant(company.Id);

            string wwwRootPath = this.hostingEnvironment.WebRootPath;
            string companyPath = @"images\companies\company-" + id;
            string finalPath = Path.Combine(wwwRootPath, companyPath);
            if(Directory.Exists(finalPath))
                Directory.Delete(finalPath, true);

            return View();
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
            var objFromDb = this.unitOfWork.Company.Get(c => c.Id == id);
            if (objFromDb is null)
                return Json(new { success = false, message = "Une erreur est survenue lors de l'activation/la désactivation de l'entreprise" });
            if(objFromDb.IsEnable == false)
                objFromDb.IsEnable = true;
            else
                objFromDb.IsEnable = false;

            this.unitOfWork.Company.Update(objFromDb);
            this.unitOfWork.Save();

            return Json(new { success = true, message = "Activation/désactivation de l'entreprise réussie" });
        }

        [HttpPost]
        public IActionResult LockUnlockCompanyFunctionnalities([FromBody]string companydId, [FromBody] string functionnalityId)
        {
            return View();
        }
        #endregion
    }

    public class CompanyFunctionnalities
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool HasAccess { get; set; }
    }
}
