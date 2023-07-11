using Microsoft.AspNetCore.Mvc.Rendering;
using SaaS.Domain.Work;
using System.ComponentModel.DataAnnotations;

namespace SaaS.ViewModels.Application.DailyHours
{
    public class DailyHoursViewModel
    {
        [Required(ErrorMessage = "Veuillez entrer une date pour vos heures effectuées")]
        [Display(Name = "Date des heures effectuées")]
        public DateTime Date { get; set; }

        #region Morning
        public DateTime MorningStart { get; set; }
        public DateTime MorningEnd { get; set; }

        public IEnumerable<SelectListItem> ListOfWorkSites { get; set; }

        public int Test { get; set; }

        public List<int> MorningWorkSites { get; set; } = new();

        public List<SelectListItem> MorningWorkManagers { get; set; }
        #endregion
        #region Evening
        public DateTime EveningStart { get; set; }
        public DateTime EveningEnd { get; set; }
        public List<WorkSite> EveningWorkSites { get; set; } = new List<WorkSite>();
        public List<Domain.Identity.User> EveningWorkManagers { get; set; } = new List<Domain.Identity.User>();
        #endregion
        #region Sup
        #endregion
        public string Description { get; set; } = string.Empty;
        public bool Lunchbox { get; set; }
    }
}
