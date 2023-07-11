namespace SaaS.Domain.MessagingSystem
{
    public class ConversationMessage
    {
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        public string ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        public string SenderId { get; set; }
        /*public virtual ApplicationUser Sender { get; set; }*/
    }
}
