namespace SaaS.ViewModels.SuperCompany.Company
{
    public class CreateCompanyViewModel
    {
        public Domain.PIPL.Company Company { get; set; } = new Domain.PIPL.Company();

        public string Server { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string ConnectionString 
        {
            get
            {
                return $"Data Source={this.Server};Initial Catalog={this.Database};Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            } 
        }
    }
}
