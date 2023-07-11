﻿using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Company;

namespace SaaS.DataAccess.Repository.Company.IRepository
{
    public interface ICompanyFunctionnalitiesRepository : IApplicationRepository<CompanyFunctionnalities>
    {
        public void Update(CompanyFunctionnalities functionnalities);
    }
}
