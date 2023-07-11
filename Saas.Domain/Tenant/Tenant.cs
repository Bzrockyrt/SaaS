using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Tenant
{
    public class Tenant
    {
        [Key]
        public string Id { get; set; }

        /*public string CompanyId { get; set; }
        public virtual Company Company { get; set; }*/

        public string ServerName { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Trusted_Connection { get; set; } = string.Empty;

        public string Multiple_Active_Result_Sets { get; set; } = string.Empty;

        public string ConnectionString
        {
            get
            {
                return $"Server={ServerName};Database={DatabaseName};Trusted_Connection={Trusted_Connection};MultipleActiveResultSets={Multiple_Active_Result_Sets}";
            }
        }
    }
}
