using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class User_Authorization_Profile : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public virtual User? User { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [ValidateNever]
        public virtual Profile? Profile { get; set; }

        public int? AuthorizationId { get; set; }
        [ForeignKey(nameof(AuthorizationId))]
        [ValidateNever]
        public virtual Authorization? Authorization { get; set; }
    }
}
