using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
{
    public class WorkHour : ModelBase
    {
        [ValidateNever]
        /*public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public virtual ApplicationUser User { get; set; }*/

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

        [Required]
        [Display(Name = "Panier repas")]
        public bool LunchBox { get; set; }

        [Required]
        [Display(Name = "Commentaire")]
        public string Comment { get; set; } = string.Empty;

        [Display(Name = "Photos du chantier")]
        [ValidateNever]
        public List<WorkHourImage> WorkHourImages { get; set; }

        [ValidateNever]
        public IList<WorkHour_WorkSite> WorkHour_WorkSites { get; set; } = new List<WorkHour_WorkSite>();
    }
}
