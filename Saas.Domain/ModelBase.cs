using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain
{
    public class ModelBase
    {
        public ModelBase()
        {
            if (string.IsNullOrEmpty(this.Id))
                this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Créateur")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de création")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Display(Name = "Modificateur")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de modification")]
        public DateTime? UpdatedOn { get; set; }

        public bool IsEnable { get; set; } = true;
    }
}
