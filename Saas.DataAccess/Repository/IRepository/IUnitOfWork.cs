namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationRoleRepository ApplicationRole { get; }
        ICompanyRepository Company { get; }
        ICompanyPictureRepository CompanyPicture { get; }
        IFunctionnalityRepository Functionnality { get; }
        IUserRepository User { get; }
        IWorkSiteRepository WorkSite { get; }
        IWorkHourRepository WorkHour { get; }

        void Save();
    }
}
