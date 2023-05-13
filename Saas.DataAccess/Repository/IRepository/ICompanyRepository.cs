using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
    }
}
