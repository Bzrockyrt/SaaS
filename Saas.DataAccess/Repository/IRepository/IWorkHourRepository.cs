using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IWorkHourRepository : IRepository<WorkHour>
    {
        public void Update(WorkHour workHour);
    }
}
