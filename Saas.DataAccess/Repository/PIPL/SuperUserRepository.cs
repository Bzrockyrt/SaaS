using Microsoft.Data.SqlClient;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.PIPL;
using System.Security.Claims;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class SuperUserRepository : SuperCompanyRepository<User>, ISuperUserRepository
    {
        private readonly PIPLDeveloppementDbContext context;

        public SuperUserRepository(PIPLDeveloppementDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Update(User superUser)
        {
            this.context.Update(superUser);
        }
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
    }
}
