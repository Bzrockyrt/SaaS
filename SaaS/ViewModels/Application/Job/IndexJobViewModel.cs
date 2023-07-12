namespace SaaS.ViewModels.Application.Job
{
    public class IndexJobViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EmployeesNumber { get; set; }
        public string SubsidiaryName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
    }
}
