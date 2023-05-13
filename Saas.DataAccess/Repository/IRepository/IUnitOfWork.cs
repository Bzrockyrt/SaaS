namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IArticleImageRepository ArticleImage { get; }
        IArticleRepository Article { get; }
        IAuthorizationRepository Authorization { get; }
        ICompanyRepository Company { get; }
        IDepartmentRepository Department { get; }
        IFunctionnalityRepository Functionnality { get; }
        IGenderRepository GenderRepository { get; }
        IProfileRepository Profile { get; }
        ISubscriptionRepository Subscription { get; }
        IUserRepository User { get; }
        IWorkSiteRepository WorkSite { get; }

        void Save();
    }
}
