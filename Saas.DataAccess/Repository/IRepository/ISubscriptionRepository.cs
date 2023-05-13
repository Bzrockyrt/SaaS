using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        public void Update(Subscription subscription);
    }
}
