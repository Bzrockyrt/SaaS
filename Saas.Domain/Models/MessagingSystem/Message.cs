using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models.MessagingSystem
{
    public class Message : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public DateTime WritingOn { get; set; } = DateTime.Now;

        public DateTime ReadedOn { get; set; }
        public bool IsRead => this.ReadedOn != DateTime.MinValue;

        public int? MessageTypeId { get; set; }
        public virtual MessageType? MessageType { get; set; }

        public int? MessagePriorityId { get; set; }
        public virtual MessagePriority? MessagePriority { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
