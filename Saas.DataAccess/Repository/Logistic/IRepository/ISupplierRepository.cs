using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Logistic;

namespace SaaS.DataAccess.Repository.Logistic.IRepository
{
    public interface ISupplierRepository : IApplicationRepository<Supplier>
    {
        public void Update(Supplier supplier);
    }
}
