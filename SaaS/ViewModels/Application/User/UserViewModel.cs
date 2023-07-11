namespace SaaS.ViewModels.Application.User
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
    }
}
