using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaS.Domain.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class WorkHour : BaseModel
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public virtual User User { get; set; }

        [Required]
        [Display(Name = "Jour de travail")]
        public DateTime WorkDay { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Début du matin")]
        public TimeSpan MorningStart { get; set; }

        [Required]
        [Display(Name = "Fin du matin")]
        public TimeSpan MorningEnd { get; set; }

        [Required]
        [Display(Name = "Début de l'après-midi")]
        public TimeSpan EveningStart { get; set; }

        [Required]
        [Display(Name = "Fin de l'après-midi")]
        public TimeSpan EveningEnd { get; set; }

        [Display(Name = "Photos du chantier")]
        [ValidateNever]
        public List<WorkHourImage> WorkHourImages { get; set; }

        [ValidateNever]
        public IList<WorkHour_WorkSite> WorkHour_WorkSites { get; set; } = new List<WorkHour_WorkSite>();
    }
}
