using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company.IRepository
{
    public interface ICompanyPictureRepository : IApplicationRepository<CompanyPicture>
    {
        public void Update(CompanyPicture companyPicture);
    }
}
