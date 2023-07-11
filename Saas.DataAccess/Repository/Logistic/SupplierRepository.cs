using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Logistic.IRepository;
using SaaS.Domain.Logistic;

namespace SaaS.DataAccess.Repository.Logistic
{
    public class SupplierRepository : ApplicationRepository<Supplier>, ISupplierRepository
    {
        private readonly ApplicationDbContext context;

        public SupplierRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Supplier supplier)
        {
            context.Update(supplier);
        }
    }
}
