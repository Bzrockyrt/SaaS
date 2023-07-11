using Microsoft.AspNetCore.Mvc;

namespace SaaS.Areas.SuperCompany.Controllers
{
    [Area("SuperCompany")]
    public class ServicingController : Controller
    {
        /*Contenu du controller Servicing : 
            - Une liste de toutes les entreprises pour demander l'accès à leur compte entreprise
            - Lorsque j'accède au log de l'entreprise, j'ai accès à toutes les informations, avertissements et erreurs de l'application
            - Je peux alors créer un message général dans tous les comptes utilisateurs pour les informer d'un changement
        */
        public IActionResult Index()
        {
            return View();
        }
    }
}
