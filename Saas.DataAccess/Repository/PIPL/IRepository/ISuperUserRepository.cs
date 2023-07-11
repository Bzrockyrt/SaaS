using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface ISuperUserRepository : ISuperCompanyRepository<User>
    {
        public void Update(User user);
    }
}
