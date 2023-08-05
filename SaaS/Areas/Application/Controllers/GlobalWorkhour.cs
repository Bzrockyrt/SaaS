using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.Domain.Work;
using SaaS.ViewModels.Application.Department;
using SaaS.ViewModels.Application.GlobalWorkhour;
using SaaS.ViewModels.Application.WorkHour;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class GlobalWorkhour : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexGlobalWorkhourViewModel> GlobalWorkHours = new List<IndexGlobalWorkhourViewModel>();

        public GlobalWorkhour(IApplicationUnitOfWork applicationUnitOfWork,
            TenantService tenantService,
            IOptions<TenantSettings> options)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
            this.tenantService = tenantService;
            this.tenantSettings = options.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAllWorkhours()
        {
            GlobalWorkHours.Clear();
            if (this.tenantSettings.Companies is not null)
            {
                foreach (TenantData tenantData in this.tenantSettings.Companies.Values)
                {
                    if (tenantData.name == this.tenantService.GetTenantName())
                    {
                        using (SqlConnection connection = new SqlConnection(tenantData.connectionString))
                        {
                            try
                            {
                                connection.Open();
                                var query = "SELECT WorkHour.Id, WorkHour.WorkDay, WorkHour.MorningStart, WorkHour.MorningEnd, WorkHour.EveningStart, WorkHour.EveningEnd, WorkHour.LunchBox, WorkHour.Comment, AspNetUsers.UserName FROM AspNetUsers INNER JOIN WorkHour ON AspNetUsers.Id = WorkHour.UserId;";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    GlobalWorkHours.Add(new IndexGlobalWorkhourViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        WorkDay = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1),
                                        MorningStart = reader.IsDBNull(2) ? TimeSpan.Zero : reader.GetTimeSpan(2),
                                        MorningEnd = reader.IsDBNull(3) ? TimeSpan.Zero : reader.GetTimeSpan(3),
                                        EveningStart = reader.IsDBNull(4) ? TimeSpan.Zero : reader.GetTimeSpan(4),
                                        EveningEnd = reader.IsDBNull(5) ? TimeSpan.Zero : reader.GetTimeSpan(5),
                                        Lunchbox = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                        Comment = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                        UserName = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = GlobalWorkHours });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des heures des salariés", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération heures salariés";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des heures des salariés";
                                return Json(null);
                            }
                        }
                    }
                }
                return Json(null);
            }
            return Json(null);

            /*foreach (WorkHour wh in this.applicationUnitOfWork.WorkHour.GetAll().Where(wh => wh.IsEnable))
            {
                GlobalWorkHours.Add(new IndexGlobalWorkhourViewModel
                {
                    Id = wh.Id,
                    Comment = wh.Comment,
                    CreatedOn = wh.CreatedOn,
                    EveningEnd = wh.EveningEnd,
                    EveningStart = wh.EveningStart,
                    Lunchbox = wh.LunchBox,
                    MorningEnd = wh.MorningEnd,
                    MorningStart = wh.MorningStart,
                    WorkDay = wh.WorkDay
                });
            }
            return Json(new { data = GlobalWorkHours });*/
        }
        #endregion
    }
}
