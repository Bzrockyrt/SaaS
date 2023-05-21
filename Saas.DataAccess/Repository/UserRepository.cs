using Microsoft.EntityFrameworkCore.Internal;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Models.Account;

namespace SaaS.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(User user)
        {
            this.context.Update(user);
        }

        /*public bool HasAuthorization(User user, Authorization authorization)
        {
            return user.ListAuthorizations.FirstOrDefault(ua => ua.Authorization == authorization) != null;
        }*/

        /*public bool HasAuthorization(User user, Authorization authorization)
        {
            bool hasAuthorization = this.context.User
                .Join(this.context.UserAuthorization, user => user.Id, userAuthorization => userAuthorization.UserId, (user, userAuthorization) => new { User = user, UserAuthorization = userAuthorization })
                .Any(joinResult => joinResult.User.Id == user.Id 
                    && joinResult.UserAuthorization.AuthorizationId == authorization.Id);
            return false;
        }*/
    }
}
