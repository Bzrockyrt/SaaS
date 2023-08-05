namespace SaaS.ViewModels.SuperCompany.Company
{
    public class CreateCompanyViewModel
    {
        public Domain.PIPL.Company Company { get; set; } = new Domain.PIPL.Company();

        public IList<Domain.PIPL.Functionnality> HaveFunctionnalities { get; set; } = new List<Domain.PIPL.Functionnality>();
        public IList<Domain.PIPL.Functionnality> DontHaveFunctionnalities { get; set; } = new List<Domain.PIPL.Functionnality>();

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
