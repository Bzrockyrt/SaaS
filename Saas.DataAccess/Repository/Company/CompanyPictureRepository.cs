using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Company.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company
{
    public class CompanyPictureRepository : ApplicationRepository<CompanyPicture>, ICompanyPictureRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyPictureRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(CompanyPicture companyPicture)
        {
            context.Update(companyPicture);
        }
    }
}
