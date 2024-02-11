using System.Text.RegularExpressions;

namespace ElectroWorld.Website.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateId(this string input)
        {
            // Remove special characters and convert to lowercase
            string cleanedString = Regex.Replace(input, "[^a-zA-Z0-9]+", " ").Trim();

            // Replace spaces with hyphens
            string urlFriendlyString = cleanedString.Replace(' ', '-');

            // Trim to the specified maximum length
            urlFriendlyString = urlFriendlyString.Length <= 150 ? urlFriendlyString : urlFriendlyString.Substring(0, 150);

            return urlFriendlyString;
        }
    }
}
