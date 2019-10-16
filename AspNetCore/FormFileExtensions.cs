namespace Core
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using static System.StringComparison;

    public static class FormFileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            var imageExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            return file.ContentType.Contains("image", OrdinalIgnoreCase) ||
                   imageExtensions.Any(x => file.FileName?.EndsWith(x, OrdinalIgnoreCase) == true);
        }
    }
}
