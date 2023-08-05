using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface ISuperUserRepository : ISuperCompanyRepository<User>
    {
        public void Update(User user);
        public bool CanUserAccessFunctionnality(string functionnalityCode, ClaimsPrincipal user);
    }
}
