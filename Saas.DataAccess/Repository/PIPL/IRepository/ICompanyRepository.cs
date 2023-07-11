namespace SaaS.DataAccess.Repository.PIPL.IRepository
{
    public interface ICompanyRepository : ISuperCompanyRepository<Domain.PIPL.Company>
    {
        public void Update(Domain.PIPL.Company company);
    }
}
