using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface IDepartmentRepository : IApplicationRepository<Department>
    {
        public void Update(Department department);
    }
}
