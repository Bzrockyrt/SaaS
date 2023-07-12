namespace SaaS.ViewModels.Application.User
{
    public class IndexUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
        public string JobName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string SubsidiaryName { get; set; } = string.Empty;
    }
}
