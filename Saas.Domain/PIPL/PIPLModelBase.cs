using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.PIPL
{
    public class PIPLModelBase
    {
        public PIPLModelBase()
        {
            if (string.IsNullOrEmpty(this.Id))
                this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; } = string.Empty;

        [ValidateNever]
        public virtual string CreatorId { get; set; }


        [Display(Name = "Date et heure de création")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Display(Name = "Modificateur")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de modification")]
        public DateTime? UpdatedOn { get; set; }

        public bool IsEnable { get; set; } = true;
    }
}
