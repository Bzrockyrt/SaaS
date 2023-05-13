using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IProfileRepository : IRepository<Profile>
    {
        public void Update(Profile profile);
    }
}
