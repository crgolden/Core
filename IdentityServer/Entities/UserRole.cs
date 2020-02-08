namespace Core.Entities
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    [PublicAPI]
    public class UserRole : IdentityUserRole<Guid>
    {
        /// <summary>Gets or sets the user.</summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

        /// <summary>Gets or sets the role.</summary>
        /// <value>The role.</value>
        public virtual Role Role { get; set; }
    }
}
