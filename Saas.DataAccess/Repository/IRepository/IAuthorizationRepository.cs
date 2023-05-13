using System.Net;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IAuthorizationRepository : IRepository<Authorization>
    {
        public void Update(Authorization authorization);
    }
}
