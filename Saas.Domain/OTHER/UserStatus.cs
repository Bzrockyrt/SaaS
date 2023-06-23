﻿namespace SaaS.Domain.OTHER
{
    public class UserStatus : ModelBase
    {
        public UserStatus() : base()
        {
            
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public IList<User> Users { get; set; } = new List<User>();
    }
}
