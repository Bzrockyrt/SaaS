using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Company
{
    public class CompanySetting : ModelBase
    {
        public CompanySetting() : base()
        {
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString();
        }

        [Required]
        public bool IsDemo { get; set; } = true;
    }
}
