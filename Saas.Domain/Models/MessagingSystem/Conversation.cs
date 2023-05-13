namespace SaaS.Domain.Models.MessagingSystem
{
    public class Conversation
    {
        public DateTime LastUpdated { get; set; }

        public byte[] Data { get; set; }

        public ICollection<User> Participants { get; set; }

        public ICollection<ConversationMessage> Messages { get; set; }
    }
}
