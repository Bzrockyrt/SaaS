using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.ViewModels.Application.Connection;

namespace SaaS.Areas.WebSite.Controllers
{
    [Area("WebSite")]
    public class HomePageController : Controller
    {
        public HomePageController()
        {
        }

        public IActionResult HomePage()
        {
            return View();
        }


        #region Login
        #endregion

        #region Signup
        #endregion
    }
}
