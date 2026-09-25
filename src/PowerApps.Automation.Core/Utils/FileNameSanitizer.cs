using System.Text;

namespace PowerApps.Automation.Core.Utils;

public static class FileNameSanitizer
{
    public static string Sanitize(string value)
    {
        var invalid = Path.GetInvalidFileNameChars().ToHashSet();
        var builder = new StringBuilder(value.Length);

        foreach (var character in value)
        {
            if (invalid.Contains(character) || char.IsWhiteSpace(character))
            {
                builder.Append('-');
            }
            else
            {
                builder.Append(char.ToLowerInvariant(character));
            }
        }

        var sanitized = builder.ToString().Trim('-');
        return string.IsNullOrWhiteSpace(sanitized) ? "scenario" : sanitized;
    }
}
