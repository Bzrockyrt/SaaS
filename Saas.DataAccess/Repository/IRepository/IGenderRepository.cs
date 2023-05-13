using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IGenderRepository : IRepository<Gender>
    {
        public void Update(Gender gender);
    }
}
