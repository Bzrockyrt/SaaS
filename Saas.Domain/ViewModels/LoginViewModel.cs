using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Une adresse mail est requise")]
        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Nombre de caractères minimum : 6")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Se souvenir de moi ?")]
        public bool RememberMe { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public string ReturnUrl { get; set; } = string.Empty;
    }
}
