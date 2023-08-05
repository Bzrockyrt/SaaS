using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.Domain;
using SaaS.Domain.Company;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.Company
{
    public class LogRepository : ApplicationRepository<Log>, ILogRepository
    {
        private readonly ApplicationDbContext context;

        public LogRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Log log)
        {
            context.Update(log);
        }

        public void CreateNewEventInlog(Exception? ex, ClaimsPrincipal user, string devNote = "",
            string exceptionName = "Exception", LogType logType = LogType.Information)
        {
            context.Log.Add(new Log
            {
                ExceptionName = exceptionName,
                Message = ex is null ? "Aucun message" : ex?.Message,
                Source = ex is null ? "Aucune source" : ex?.Source,
                DevNote = devNote,
                LogType = logType,
                CreatedOn = DateTime.Now,
                CreatorId = user is null ? "" : user?.Identity.Name,
                IsEnable = true,
            });
            context.SaveChanges();
        }
    }
}
