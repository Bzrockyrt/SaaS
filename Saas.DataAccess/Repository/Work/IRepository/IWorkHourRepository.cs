using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work.IRepository
{
    public interface IWorkHourRepository : IApplicationRepository<WorkHour>
    {
        public void Update(WorkHour workHour);
    }
}
