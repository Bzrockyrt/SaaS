using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Work.IRepository;
using SaaS.Domain.Work;

namespace SaaS.DataAccess.Repository.Work
{
    public class WorkHourRepository : ApplicationRepository<WorkHour>, IWorkHourRepository
    {
        private readonly ApplicationDbContext context;

        public WorkHourRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(WorkHour workHour)
        {
            context.Update(workHour);
        }
    }
}
