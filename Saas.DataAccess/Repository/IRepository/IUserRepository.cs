using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public void Update(User user);
    }
}
