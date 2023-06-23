using SaaS.Domain.Models;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
        public bool IsSuperCompany(ClaimsPrincipal user);
        public bool HasFunctionnality(Company company, Functionnality functionnality);
    }
}
