using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository
{
    public class WorkHourRepository : Repository<WorkHour>, IWorkHourRepository
    {
        private readonly ApplicationDbContext context;

        public WorkHourRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(WorkHour workHour)
        {
            this.context.Update(workHour);
        }
    }
}
