namespace Microsoft.AspNetCore.Authentication
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using static System.Threading.Tasks.Task;
    using static Microsoft.Net.Http.Headers.HeaderNames;

    /// <summary>A class with methods that extend <see cref="RemoteFailureContext"/>.</summary>
    [PublicAPI]
    public static class RemoteFailureContextExtensions
    {
        /// <summary>Handles the remote failure.</summary>
        /// <param name="context">The context.</param>
        /// <returns>A task.</returns>
        public static Task HandleRemoteFailure(this RemoteFailureContext context)
        {
            if (context == default)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Properties.Items.ContainsKey(Referer))
            {
                context.Response.Redirect($"{context.Properties.Items[Referer]}");
            }

            context.HandleResponse();
            return CompletedTask;
        }
    }
}
