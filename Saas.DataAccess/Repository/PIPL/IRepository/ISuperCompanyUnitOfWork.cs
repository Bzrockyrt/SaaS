namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface ISuperCompanyUnitOfWork
    {
        ICompanyRepository Company { get; }
        IFunctionnalityRepository Functionnality { get; }
        ILogRepository Log { get; }
        ISuperUserRepository User { get; }

        void Save();
    }
}
