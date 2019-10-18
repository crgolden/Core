namespace Core
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using static System.StringComparison;

    public static class FormFileExtensions
    {
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
