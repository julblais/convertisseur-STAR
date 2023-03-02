using NUnit.Framework;
using System.IO;

namespace STAR.Tests
{
    static class InputSource
    {
        static InputSource()
        {
            string testDir = TestContext.CurrentContext.TestDirectory;
            DirectoryInfo parent = Directory.GetParent(testDir);
            while (parent.Name != TestFolderName)
                parent = Directory.GetParent(parent.FullName);
            WorkingDir = parent.FullName;
            ResultsFolder = $"{Directory.GetParent(WorkingDir).FullName}{separator}{ResultsFolderName}{separator}";

            Directory.CreateDirectory(ResultsFolder);
        }

        public const string TestFolderName = "tests";
        public const string ResultsFolderName = "results";

        static readonly char separator = Path.DirectorySeparatorChar;
        public static string WorkingDir { get; private set; } = string.Empty;
        public static string ResultsFolder { get; private set; } = string.Empty;
        public static string CasesFolder => $"{WorkingDir}{separator}cases{separator}";
        public static string OriginalFolder => $"{CasesFolder}original{separator}";
        public static string ExpectedFolder => $"{CasesFolder}expected{separator}";

    }
}
