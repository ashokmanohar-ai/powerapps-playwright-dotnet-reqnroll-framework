namespace PowerApps.Automation.Core.Configuration;

public static class EnvFile
{
    public static void LoadIfPresent(string fileName = ".env", int maxParentDepth = 6)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        for (var depth = 0; depth <= maxParentDepth && directory is not null; depth++, directory = directory.Parent)
        {
            var path = Path.Combine(directory.FullName, fileName);
            if (!File.Exists(path))
            {
                continue;
            }

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                {
                    continue;
                }

                var separator = line.IndexOf('=');
                if (separator <= 0)
                {
                    continue;
                }

                var key = line[..separator].Trim();
                var value = line[(separator + 1)..].Trim();

                if ((value.StartsWith('"') && value.EndsWith('"')) ||
                    (value.StartsWith('\'') && value.EndsWith('\'')))
                {
                    value = value[1..^1];
                }

                if (Environment.GetEnvironmentVariable(key) is null)
                {
                    Environment.SetEnvironmentVariable(key, value);
                }
            }

            return;
        }
    }
}
