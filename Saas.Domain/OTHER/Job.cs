namespace SaaS.Domain.OTHER
{
    public class Job : ModelBase
    {
        public Job() : base()
        {
            
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public virtual string DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public IList<User> Users { get; set; } = new List<User>();
    }
}
