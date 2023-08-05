using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaS.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Work
{
    public class WorkHour : ModelBase
    {
        public WorkHour() : base()
        {

        }

        [Required]
        [Display(Name = "Jour de travail")]
        public DateTime WorkDay { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Début du matin")]
        public TimeSpan MorningStart { get; set; }

        [Required]
        [Display(Name = "Fin du matin")]
        public TimeSpan MorningEnd { get; set; }

        [NotMapped]
        [Display(Name = "Heures total matin")]
        public TimeSpan TotalMorningHours 
        {
            get
            {
                return MorningEnd - MorningStart;
            }
        }

        [Required]
        [Display(Name = "Début de l'après-midi")]
        public TimeSpan EveningStart { get; set; }

        [Required]
        [Display(Name = "Fin de l'après-midi")]
        public TimeSpan EveningEnd { get; set; }

        [NotMapped]
        [Display(Name = "Heures total après-midi")]
        public TimeSpan TotalEveningHours
        {
            get
            {
                return EveningEnd - EveningStart;
            }
        }

        [Required]
        [Display(Name = "Panier repas")]
        public bool LunchBox { get; set; }

        [Required]
        [Display(Name = "Commentaire")]
        public string Comment { get; set; } = string.Empty;

        [ValidateNever]
        public virtual string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual User User { get; set; }

        [Display(Name = "Photos du chantier")]
        [ValidateNever]
        public List<WorkHourImage> WorkHourImages { get; set; }

        [ValidateNever]
        public IList<WorkHour_WorkSite> WorkHour_WorkSites { get; set; } = new List<WorkHour_WorkSite>();

        /*[ValidateNever]
        public IList<WorkHour_WorkManager> WorkHours_WorkManagers { get; set; } = new List<WorkHour_WorkManager>();*/
    }
}
