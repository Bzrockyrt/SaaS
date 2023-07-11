using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class LogRepository : SuperCompanyRepository<Log>, ILogRepository
    {
        private readonly PIPLDeveloppementDbContext context;

        public LogRepository(PIPLDeveloppementDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(Log log)
        {
            this.context.Update(log);
        }

        public void CreateNewEventInlog(Exception? ex, ClaimsPrincipal user, string devNote = "",
            string exceptionName = "Exception", LogType logType = LogType.Information)
        {
            this.context.Log.Add(new Domain.PIPL.Log
            {
                ExceptionName = exceptionName,
                Message = ex is null ? "Aucun message" : ex?.Message,
                Source = ex is null ? "Aucune source" : ex?.Source,
                DevNote = devNote,
                LogType = logType,
                CreatedOn = DateTime.Now,
                CreatedBy = user is not null ? "Non renseigné" : user?.Identity.Name,
                IsEnable = true,
            });
            this.context.SaveChanges();
        }
    }
}
