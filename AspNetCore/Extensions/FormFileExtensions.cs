namespace Microsoft.AspNetCore.Http
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using static System.StringComparison;

    /// <summary>A class with methods that extend <see cref="IFormFile"/>.</summary>
    [PublicAPI]
    public static class FormFileExtensions
    {
        /// <summary>Determines whether <paramref name="file"/> is an image.</summary>
        /// <param name="file">The file.</param>
        /// <returns><c>true</c> if the specified file is an image; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is <see langword="null" />.</exception>
        public static bool IsImage(this IFormFile file)
        {
            if (file == default)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var imageExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            return file.ContentType.Contains("image", OrdinalIgnoreCase) ||
                   imageExtensions.Any(x => file.FileName?.EndsWith(x, OrdinalIgnoreCase) == true);
        }
    }
}
