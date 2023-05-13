using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
{
    public class Gender : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom du genre doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du genre ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description du genre doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description du genre ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        public IList<User> Users { get; set; } = new List<User>();
    }
}
