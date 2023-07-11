using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface IJobRepository : IApplicationRepository<Job>
    {
        public void Update(Job job);
    }
}
