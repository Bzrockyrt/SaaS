using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaS.Domain.PIPL;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.OTHER
{
    public class CompanyPicture : ModelBase
    {
        public CompanyPicture() : base()
        {
            if (string.IsNullOrEmpty(this.Id))
                Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public virtual string CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public virtual Company Company { get; set; }
    }
}
