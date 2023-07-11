using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.Company;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class CompanyRepository : SuperCompanyRepository<Domain.PIPL.Company>, ICompanyRepository
    {
        private readonly PIPLDeveloppementDbContext context;

        public CompanyRepository(PIPLDeveloppementDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(Domain.PIPL.Company company)
        {
            this.context.Update(company);
        }

        public bool HasRight(string right)
        {
            return false;
        }

        public bool HasFunctionnality(Domain.PIPL.Company company, Functionnality functionnality)
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
