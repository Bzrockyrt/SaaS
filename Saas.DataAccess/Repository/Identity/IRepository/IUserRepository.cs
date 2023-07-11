﻿using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.Identity.IRepository
{
    public interface IUserRepository : IApplicationRepository<User>
    {
        public void Update(User user);
        public bool CanUserAccessFunctionnality(string functionnalityCode, ClaimsPrincipal user);
    }
}
