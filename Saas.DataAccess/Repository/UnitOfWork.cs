using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IApplicationRoleRepository ApplicationRole { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public ICompanyPictureRepository CompanyPicture { get; private set; }
        public IFunctionnalityRepository Functionnality { get; private set; }
        public IUserRepository User { get; private set; }
        public IWorkSiteRepository WorkSite { get; private set; }
        public IWorkHourRepository WorkHour { get; private set; }

        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            this.ApplicationRole = new ApplicationRoleRepository(this.context);
            /*this.Company = new CompanyRepository(this.context);*/
            this.CompanyPicture = new CompanyPictureRepository(this.context);
            this.Functionnality = new FunctionnalityRepository(this.context);
            this.User = new UserRepository(this.context);
            this.WorkSite = new WorkSiteRepository(this.context);
            this.WorkHour = new WorkHourRepository(this.context);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}
