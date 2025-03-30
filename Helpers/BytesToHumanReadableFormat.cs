namespace OllamaManager.Helpers;

public static class BytesToHumanReadableFormat
{
    private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    public static string Convert(long bytes)
    {
        double size = bytes;
        var order = 0;

        while (size >= 1024 && order < SizeSuffixes.Length - 1)
        {
            size /= 1024;
            order++;
        }

        return $"{size:0.##} {SizeSuffixes[order]}";
    }
}
