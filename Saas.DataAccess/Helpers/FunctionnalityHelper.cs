using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SaaS.DataAccess.Repository.IRepository;
using System.Security.Principal;

namespace SaaS.DataAccess.Helpers
{
    public static class FunctionnalityHelper
    {
        private static readonly IUnitOfWork unitOfWork;

        public static bool HasFunctionnality(string functionnalityName, IPrincipal user)
        {
            /*Je dois vérifier si l'utilisateur a accès à cette fonctionnalité ou non
              Pour cela, je vérifie le rôle/profil de l'utilisateur.
              Après avoir récupéré son ou ses rôles, je vérifie que parmis son ou ses rôles,
              au moins un d'entre eux possède la fonctionnalité passée en paramètre.
              
              Si c'est le cas, l'utilisateur a accès à cette fonctionnalité.
              Si ce n'est pas le cas, l'utilisateur n'a pas accès à cette fonctionnalité.*/

            //Je récupère l'utilisateur actuel
            var currentUser = unitOfWork.User.Get(u => u.NormalizedUserName == user.Identity.Name);

            /*var userProfile = unitOfWork.UserRole.GetAll().Where(ur => ur.UserId == currentUser.Id && ur.RoleId == ).Any();*/

            return user.IsInRole("Admin");
        }
    }
}
