using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Company
{
    public class CompanyPicture : ModelBase
    {
        public CompanyPicture() : base()
        {
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
