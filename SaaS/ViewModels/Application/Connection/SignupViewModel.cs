using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SaaS.ViewModels.Application.Connection
{
    public class SignupViewModel
    {
        [MinLength(3, ErrorMessage = "Votre prénom doit comporter au moins 3 caractères")]
        [Required(ErrorMessage = "Un prénom est requis")]
        public string Firstname { get; set; } = string.Empty;

        [MinLength(3, ErrorMessage = "Votre nom doit comporter au moins 3 caractères")]
        [Required(ErrorMessage = "Un nom est requis")]
        public string Lastname { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Adresse mail invalide")]
        [Required(ErrorMessage = "Une adresse mail est requise")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Un mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Nombre minimum de caractères : 6")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez confirmer votre mot de passe")]
        [MinLength(6, ErrorMessage = "Nombre minimum de catactères : 6")]
        [Compare("Password", ErrorMessage = "Les mots de passes ne correspondent pas")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsAgree { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public string ReturnUrl { get; set; } = string.Empty;
    }
}
