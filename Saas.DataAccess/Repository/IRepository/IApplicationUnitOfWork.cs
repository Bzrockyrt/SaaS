using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.DataAccess.Repository.Logistic.IRepository;
using SaaS.DataAccess.Repository.Work.IRepository;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IApplicationUnitOfWork
    {
        ICompanyFunctionnalitiesRepository CompanyFunctionnalities { get; }
        ICompanyPictureRepository CompanyPicture { get; }
        ICompanySettingRepository CompanySetting { get; }
        IDepartmentRepository Department { get; }
        IEmploymentContractRepository EmploymentContract { get; }
        IJobRepository Job { get; }
        IJob_CompanyFunctionnalitiesRepository Job_CompanyFunctionnalities { get; }
        ILogRepository Log { get; }
        ISubsidiaryRepository Subsidiary { get; }
        ISupplierRepository Supplier { get; }
        IUserRepository User { get; }
        IWorkHourRepository WorkHour { get; }
        IWorkHour_WorkSiteRepository WorkHour_WorkSite { get; }
        IWorkSiteRepository WorkSite { get; }

        void Save();
    }
}
