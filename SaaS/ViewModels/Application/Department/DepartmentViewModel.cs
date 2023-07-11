﻿namespace SaaS.ViewModels.Application.Department
{
    public class DepartmentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int JobsNumber { get; set; }
        public int EmployeesNumber { get; set; }
        public bool IsEnable { get; set; }
    }
}
