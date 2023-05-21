using SaaS.Domain.Models;
using SaaS.Domain.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace SaaS.ViewModels.Application.DailyHours
{
    public class DailyHoursViewModel
    {
        [Required(ErrorMessage = "Veuillez entrer une date pour vos heures effectuées")]
        [Display(Name = "Date des heures effectuées")]
        public DateTime Date { get; set; }

        #region Morning
        public TimeSpan MorningStart { get; set; }
        public TimeSpan MorningEnd { get; set; }
        public List<WorkSite> MorningWorkSites { get; set; } = new List<WorkSite>();
        public List<User> MorningWorkManagers { get; set; } = new List<User>();
        #endregion
        #region Evening
        public TimeSpan EveningStart { get; set; }
        public TimeSpan EveningEnd { get; set; }
        public List<WorkSite> EveningWorkSites { get; set; } = new List<WorkSite>();
        public List<User> EveningWorkManagers { get; set; } = new List<User>();
        #endregion
    }
}
