namespace SaaS.DataAccess.Utils
{
    public class TenantSettings
    {
        public Dictionary<string, TenantData> Companies { get; set; }
    }

    public class TenantData
    {
        public string connectionString { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
    }
}
