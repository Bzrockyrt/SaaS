﻿using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models.MessagingSystem
{
    public class MessageType : ModelBase
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
