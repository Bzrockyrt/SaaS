using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity
{
    public class DepartmentRepository : ApplicationRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Department department)
        {
            context.Update(department);
        }
    }
}
