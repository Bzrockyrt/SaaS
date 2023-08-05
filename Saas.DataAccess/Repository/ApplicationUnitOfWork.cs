using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Company;
using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.DataAccess.Repository.Identity;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.DataAccess.Repository.Logistic;
using SaaS.DataAccess.Repository.Logistic.IRepository;
using SaaS.DataAccess.Repository.Work;
using SaaS.DataAccess.Repository.Work.IRepository;

namespace SaaS.DataAccess.Repository
{
    public class ApplicationUnitOfWork : IApplicationUnitOfWork
    {
        public ICompanyFunctionnalitiesRepository CompanyFunctionnalities { get; private set; }
        public ICompanyPictureRepository CompanyPicture { get; private set; }
        public ICompanySettingRepository CompanySetting { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IEmploymentContractRepository EmploymentContract { get; private set; }
        public IJobRepository Job { get; private set; }
        public IJob_CompanyFunctionnalitiesRepository Job_CompanyFunctionnalities { get; private set; }
        public ILogRepository Log { get; private set; }
        public ISubsidiaryRepository Subsidiary { get; private set; }
        public ISupplierRepository Supplier { get; private set; }
        public IUserRepository User { get; private set; }
        public IWorkHourRepository WorkHour { get; private set; }
        public IWorkHour_WorkSiteRepository WorkHour_WorkSite { get; private set; }
        public IWorkSiteRepository WorkSite { get; private set; }


        private readonly ApplicationDbContext context;

        public ApplicationUnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            CompanyFunctionnalities = new CompanyFunctionnalitiesRepository(context);
            CompanyPicture = new CompanyPictureRepository(this.context);
            CompanySetting = new CompanySettingRepository(this.context);
            Department = new DepartmentRepository(this.context);
            EmploymentContract = new EmploymentContractRepository(this.context);
            Job = new JobRepository(this.context);
            Job_CompanyFunctionnalities = new Job_CompanyFunctionnalititesRepository(this.context);
            Log = new LogRepository(this.context);
            Subsidiary = new SubsidiaryRepository(this.context);
            Supplier = new SupplierRepository(this.context);
            User = new UserRepository(this.context);
            WorkHour = new WorkHourRepository(this.context);
            WorkHour_WorkSite = new WorkHour_WorkSiteRepository(this.context);
            WorkSite = new WorkSiteRepository(this.context);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
