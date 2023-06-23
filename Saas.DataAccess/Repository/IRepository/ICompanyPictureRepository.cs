using SaaS.Domain.Models;
using SaaS.Domain.OTHER;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface ICompanyPictureRepository : IRepository<CompanyPicture>
    {
        public void Update(CompanyPicture companyPicture);
    }
}
