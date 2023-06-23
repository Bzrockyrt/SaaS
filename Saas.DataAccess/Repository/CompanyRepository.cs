using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models;
using SaaS.Domain.OTHER;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(Company company)
        {
            this.context.Update(company);
        }

        public bool HasRight(string right)
        {
            return false;
        }

        public bool HasFunctionnality(Company company, Functionnality functionnality)
        {
            if (company is not null)
            {
                /*foreach (CompanyFunctionnalities functs in company?.Fonctionnalities)
                {
                    if (functs.Name == functionnality.Name) return true;
                }*/
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsSuperCompany(ClaimsPrincipal user)
        {
            return false;
        }
    }
}
