using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
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
                return $"Server={this.ServerName};Database={this.DatabaseName};Trusted_Connection={this.Trusted_Connection};MultipleActiveResultSets={this.Multiple_Active_Result_Sets}";
            }
        }
    }
}
