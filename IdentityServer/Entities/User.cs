namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    [PublicAPI]
    public class User : IdentityUser<Guid>
    {
        /// <summary>Initializes a new instance of the <see cref="User"/> class.</summary>
        public User()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="User"/> class.</summary>
        /// <param name="userName">The user name.</param>
        public User(string userName)
            : base(userName)
        {
        }

        /// <summary>Gets the claims.</summary>
        /// <value>The claims.</value>
        public virtual ICollection<UserClaim> Claims { get; } = new List<UserClaim>();

        /// <summary>Gets the logins.</summary>
        /// <value>The logins.</value>
        public virtual ICollection<UserLogin> Logins { get; } = new List<UserLogin>();

        /// <summary>Gets the tokens.</summary>
        /// <value>The tokens.</value>
        public virtual ICollection<UserToken> Tokens { get; } = new List<UserToken>();

        /// <summary>Gets the user roles.</summary>
        /// <value>The user roles.</value>
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
    }
}
