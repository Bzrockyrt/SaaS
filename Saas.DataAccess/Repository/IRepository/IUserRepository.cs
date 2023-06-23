using SaaS.Domain.OTHER;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public void Update(User user);
        public bool CanUserAccessFunctionnality(string functionnalityCode, ClaimsPrincipal user);

    }
}
