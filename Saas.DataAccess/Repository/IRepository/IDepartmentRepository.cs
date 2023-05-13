using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        public void Update(Department department);
    }
}
