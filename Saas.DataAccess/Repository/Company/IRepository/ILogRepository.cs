using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain;
using SaaS.Domain.Company;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.Company.IRepository
{
    public interface ILogRepository : IApplicationRepository<Log>
    {
        public void Update(Log log);
        public void CreateNewEventInlog(Exception? ex, ClaimsPrincipal user, string devNote = "", string exceptionName = "Exception", LogType logType = LogType.Information);
    }
}
