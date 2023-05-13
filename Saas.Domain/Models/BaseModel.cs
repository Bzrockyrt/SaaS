using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SaaS.Domain.Models
{
    public class BaseModel
    {
        [Key] 
        public int Id { get; set; }

        [Display(Name = "Créateur")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de création")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Display(Name = "Modificateur")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de modification")]
        public DateTime? UpdatedOn { get; set; }

        public bool IsEnable { get; set; }
    }
}
