using System.IO;

static class InputSource
{
    static InputSource()
    {
        string testDir = Directory.GetCurrentDirectory();
        DirectoryInfo parent = Directory.GetParent(testDir);
        while (parent.Name != PerfFolderName)
            parent = Directory.GetParent(parent.FullName);
        WorkingDir = parent.FullName;
        PerfFolder = $"{Directory.GetParent(WorkingDir).FullName}{separator}{PerfFolderName}{separator}";
    }

    public const string perfFile = "perfSource.txt";
    public const string PerfFolderName = "performance";

    static readonly char separator = Path.DirectorySeparatorChar;
    public static string WorkingDir { get; private set; } = string.Empty;
    public static string PerfFolder { get; private set; } = string.Empty;
}
