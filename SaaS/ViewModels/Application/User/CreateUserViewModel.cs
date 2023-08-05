using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SaaS.ViewModels.Application.User
{
    public class CreateUserViewModel
    {
        public Domain.Identity.User User { get; set; }

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Nombre minimum de caractères : 6")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez confirmer votre mot de passe")]
        [MinLength(6, ErrorMessage = "Nombre minimum de catactères : 6")]
        [Compare("Password", ErrorMessage = "Les mots de passes ne correspondent pas")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public virtual string JobId { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> SubsidiaryList { get; set; }
    }
}
