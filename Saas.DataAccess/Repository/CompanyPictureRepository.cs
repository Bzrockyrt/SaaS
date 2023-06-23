using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;
using SaaS.Domain.OTHER;

namespace SaaS.DataAccess.Repository
{
    public class CompanyPictureRepository : Repository<CompanyPicture>, ICompanyPictureRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyPictureRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(CompanyPicture companyPicture)
        {
            this.context.Update(companyPicture);
        }
    }
}
