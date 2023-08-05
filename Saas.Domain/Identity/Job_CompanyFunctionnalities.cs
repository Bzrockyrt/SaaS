using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaS.Domain.Company;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Identity
{
    public class Job_CompanyFunctionnalities
    {
        public virtual string JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        [ValidateNever]
        public virtual Job Job { get; set; }

        public virtual string CompanyFunctionnalitiesId { get; set; }
        [ForeignKey(nameof(CompanyFunctionnalitiesId))]
        [ValidateNever]
        public virtual CompanyFunctionnalities CompanyFunctionnalities { get; set; }
    }
}
