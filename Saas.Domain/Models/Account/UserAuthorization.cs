namespace SaaS.Domain.Models.Account
{
    public class UserAuthorization
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to an authorization.
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the primary key of the authorization that is linked to the user.
        /// </summary>
        public int AuthorizationId { get; set; } = default!;
        public Authorization Authorization { get; set; }
    }
}
