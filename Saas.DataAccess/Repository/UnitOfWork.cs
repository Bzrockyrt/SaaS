using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;

namespace SaaS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IArticleImageRepository ArticleImage { get; private set; }
        public IArticleRepository Article { get; private set; }
        public IAuthorizationRepository Authorization { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IFunctionnalityRepository Functionnality { get; private set; }
        public IGenderRepository GenderRepository { get; private set; }
        public IProfileRepository Profile { get; private set; }
        public ISubscriptionRepository Subscription { get; private set; }
        public IUserRepository User { get; private set; }
        public IWorkSiteRepository WorkSite { get; private set; }

        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            this.Company = new CompanyRepository(this.context);
            this.User = new UserRepository(this.context);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}
