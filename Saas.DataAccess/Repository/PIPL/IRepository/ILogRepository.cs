using SaaS.Domain;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface ILogRepository : ISuperCompanyRepository<Log>
    {
        public void Update(Log log);
        public void CreateNewEventInlog(Exception? ex, ClaimsPrincipal user, string devNote = "",
            string exceptionName = "Exception", LogType logType = LogType.Information);
    }
}
