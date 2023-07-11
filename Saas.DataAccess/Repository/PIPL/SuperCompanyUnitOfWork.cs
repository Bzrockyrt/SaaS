using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class SuperCompanyUnitOfWork : ISuperCompanyUnitOfWork
    {
        public ICompanyRepository Company { get; private set; }
        public IFunctionnalityRepository Functionnality { get; private set; }
        public ILogRepository Log { get; private set; }
        public ISuperUserRepository User { get; private set; }

        private readonly PIPLDeveloppementDbContext context;

        public SuperCompanyUnitOfWork(PIPLDeveloppementDbContext context)
        {
            this.context = context;
            this.Company = new CompanyRepository(this.context);
            this.Functionnality = new FunctionnalityRepository(this.context);
            this.Log = new LogRepository(this.context);
            this.User = new SuperUserRepository(this.context);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}
