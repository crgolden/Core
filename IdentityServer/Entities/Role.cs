namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    [PublicAPI]
    public class Role : IdentityRole<Guid>
    {
        /// <summary>Initializes a new instance of the <see cref="Role"/> class.</summary>
        public Role()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Role"/> class.</summary>
        /// <param name="roleName">The role name.</param>
        public Role(string roleName)
            : base(roleName)
        {
        }

        /// <summary>Gets the user roles.</summary>
        /// <value>The user roles.</value>
        public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

        /// <summary>Gets the role claims.</summary>
        /// <value>The role claims.</value>
        public virtual ICollection<RoleClaim> RoleClaims { get; } = new List<RoleClaim>();
    }
}
