namespace Core.Entities
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    [PublicAPI]
    public class UserClaim : IdentityUserClaim<Guid>
    {
        /// <summary>Gets or sets the user.</summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }
    }
}
