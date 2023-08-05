using Microsoft.Data.SqlClient;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.Identity.IRepository;
using SaaS.Domain.Identity;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.Identity
{
    public class UserRepository : ApplicationRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(User user)
        {
            context.Update(user);
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
            string query = $"SELECT CompanyFunctionnalities.Code FROM CompanyFunctionnalities INNER JOIN Job_CompanyFunctionnalities ON CompanyFunctionnalities.Id = Job_CompanyFunctionnalities.CompanyFunctionnalitiesId INNER JOIN Job ON Job_CompanyFunctionnalities.JobId = Job.Id INNER JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId WHERE AspNetUsers.Username = '{user?.Identity.Name}';";

            using (var connection = new SqlConnection(context.ConnectionString))
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

        public string GetSubsidiaryPrimaryColor(ClaimsPrincipal user)
        {
            var query = $"SELECT Subsidiary.PrimaryColor FROM Subsidiary INNER JOIN Department ON Subsidiary.Id = Department.SubsidiaryId INNER JOIN Job ON Department.Id = Job.DepartmentId INNER JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId WHERE AspNetUsers.UserName = '{user?.Identity.Name}';";

            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.IsDBNull(0) ? "" : reader.GetString(0);
                }

                reader.Close();
            }
                return "";
        }
        public string GetSubsidiarySecondaryColor(ClaimsPrincipal user)
        {
            var query = $"SELECT Subsidiary.SecondaryColor FROM Subsidiary INNER JOIN Department ON Subsidiary.Id = Department.SubsidiaryId INNER JOIN Job ON Department.Id = Job.DepartmentId INNER JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId WHERE AspNetUsers.UserName = '{user?.Identity.Name}';";

            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.IsDBNull(0) ? "" : reader.GetString(0);
                }

                reader.Close();
            }
            return "";
        }
        public string GetSubsidiaryTertiaryColor(ClaimsPrincipal user)
        {
            var query = $"SELECT Subsidiary.TertiaryColor FROM Subsidiary INNER JOIN Department ON Subsidiary.Id = Department.SubsidiaryId INNER JOIN Job ON Department.Id = Job.DepartmentId INNER JOIN AspNetUsers ON Job.Id = AspNetUsers.JobId WHERE AspNetUsers.UserName = '{user?.Identity.Name}';";

            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return reader.IsDBNull(0) ? "" : reader.GetString(0);
                }

                reader.Close();
            }
            return "";
        }
    }
}
