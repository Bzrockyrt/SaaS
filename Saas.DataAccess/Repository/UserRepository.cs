using Microsoft.Data.SqlClient;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.OTHER;
using System.Security.Claims;

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

        /*Récupération de l'entreprise de l'utilisateur*/
        /*public Company GetUserCompany(User user)
        {
            string getUserCompanyQuery = "SELECT Company.Id" +
                                         "FROM Company" +
                                         "INNER JOIN Department" +
                                         "ON Company.Id = Department.CompanyId" +
                                         "INNER JOIN [User]" +
                                         "ON Department.Id = [User].DepartmentId" +
                                         $"WHERE [User].Id = ${user.Id}";

            SqlCommand command = new SqlCommand(getUserCompanyQuery);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int companyId = (int)reader.GetValue(0);
                }
            }

            string getCompanyQuery = $"SELECT * FROM Company WHERE Company.Id = {companyId}";
            SqlCommand getCompanyCommand = new SqlCommand(getCompanyQuery);
        }*/

        private const string getUserDepartmentQuery = "SELECT Department.Id, Department.Name" +
                                                      "FROM Department" +
                                                      "INNER JOIN User" +
                                                      "ON Department.Id = User.DepartmentId";

        /*Récupération du département de l'utilisateur*/
        /*public Department GetDepartment()
        {
            SqlCommand command = new SqlCommand();
        }*/


        /*Pour pouvoir vérifier si un utilisateur à le droit d'accéder à la fonctionnalité passée en paramètres, il y a plusieurs conditions à respecter : 
            - Vérifier quel est le rôle/profil de l'utilisateur
            - Vérifier si ce rôle possède l'autorisation d'accéder à cette fonctionnalité
            - Si c'est le cas, retourner vrai, sinon retourner faux*/
        /// <summary>
        /// Check if current user has rights to access functionnality passed in parameter
        /// </summary>
        /// <param name="functionnalityName"></param>
        /// <returns>True: current user has rights to access functionnality passed in parameter
        ///          False: current has no rights to access functionnality passed in parameter
        /// </returns>
        public bool CanUserAccessFunctionnality(string functionnalityCode, ClaimsPrincipal user)
        {
            string query = $"SELECT Functionnality.Code FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id INNER JOIN ApplicationRoleFunctionnality ON AspNetRoles.Id = ApplicationRoleFunctionnality.RoleId INNER JOIN Functionnality ON ApplicationRoleFunctionnality.FunctionnalityId = Functionnality.Id WHERE AspNetUsers.Email = '{user?.Identity?.Name}';";

            using (var connection = new SqlConnection(this.context.ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                object results = "";

                while (reader.Read())
                {
                    if (functionnalityCode == reader["Code"].ToString())
                        return true;
                }

                reader.Close();
            }

            return false;
        }
    }
}
