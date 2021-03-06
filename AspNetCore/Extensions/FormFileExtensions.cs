﻿namespace Microsoft.AspNetCore.Http
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
        /// <returns><see langword="true" /> if the specified file is an image; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is <see langword="null" />.</exception>
        public static bool IsImage(this IFormFile file)
        {
            if (file == default)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var imageExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
#if NET5_0
            return file.ContentType.Contains("image", InvariantCultureIgnoreCase) ||
                   imageExtensions.Any(x => file.FileName?.EndsWith(x, OrdinalIgnoreCase) == true);
#else
            return file.ContentType.Contains("image") ||
                   imageExtensions.Any(x => file.FileName?.EndsWith(x, OrdinalIgnoreCase) == true);
#endif
        }
    }
}
