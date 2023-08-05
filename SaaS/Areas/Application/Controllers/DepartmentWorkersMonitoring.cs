using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SaaS.DataAccess.Repository;
using SaaS.DataAccess.Services;
using SaaS.DataAccess.Utils;
using SaaS.Domain;
using SaaS.Domain.Identity;
using SaaS.ViewModels.Application.DepartmentWorkersMonitoring;

namespace SaaS.Areas.Application.Controllers
{
    public class DepartmentWorkersMonitoring : Controller
    {
        private readonly ApplicationUnitOfWork applicationUnitOfWork;
        private readonly TenantService tenantService;
        private readonly TenantSettings tenantSettings;

        public static List<IndexDepartmentWorkerMonitoringViewModel> DepartmentWorker = new List<IndexDepartmentWorkerMonitoringViewModel>();

        public DepartmentWorkersMonitoring(TenantService tenantService,
            IOptions<TenantSettings> options,
            ApplicationUnitOfWork applicationUnitOfWork)
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
        public IActionResult GetAllUsers()
        {
            DepartmentWorker.Clear();
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
                                var query = "SELECT AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.PhoneNumber, AspNetUsers.IsEnable, Job.[Name] JobName, Department.[Name] DepartmentName, Subsidiary.[Name] SubsidiaryName FROM Subsidiary RIGHT JOIN Department ON Subsidiary.Id = Department.SubsidiaryId RIGHT JOIN Job ON Department.Id = Job.DepartmentId RIGHT JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId GROUP BY AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.PhoneNumber, AspNetUsers.IsEnable, Job.[Name], Department.[Name], Subsidiary.[Name];";
                                SqlCommand cmd = new SqlCommand(query, connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    DepartmentWorker.Add(new IndexDepartmentWorkerMonitoringViewModel
                                    {
                                        Id = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        UserName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                        Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                        PhoneNumber = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                        IsEnable = reader.GetBoolean(4),
                                        JobName = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                        DepartmentName = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                        SubsidiaryName = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    });
                                }
                                reader.Close();
                                connection.Close();
                                return Json(new { data = DepartmentWorker });
                            }
                            catch (Exception ex)
                            {
                                this.applicationUnitOfWork.Log.CreateNewEventInlog(ex, User, "Une erreur est survenue lors de la récupération des employés", "Exception", LogType.Error);
                                TempData["error-title"] = "Récupération employés";
                                TempData["error-message"] = "Une erreur est survenue lors de la récupération des employés";
                                return Json(null);
                            }
                        }
                    }
                }
            }

            IEnumerable<User> users = this.applicationUnitOfWork.User.GetAll();

            return Json(new { data = users });
        }
        #endregion
    }
}
