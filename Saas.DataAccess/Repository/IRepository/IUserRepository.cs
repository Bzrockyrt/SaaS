using SaaS.Domain.Models.Account;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public void Update(User user);

        /*public bool HasAuthorization(User user, Authorization authorization);*/
    }
}
