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
            Directory.CreateDirectory(ResultsFolder);
        }

        public const string b_File = "b.txt";
        public const string d_File = "d.txt";
        public const string TestFolderName = "tests";

        public static string WorkingDir = string.Empty;
        public static string CasesFolder => $"{WorkingDir}{Path.DirectorySeparatorChar}cases{Path.DirectorySeparatorChar}";
        public static string OriginalFolder => $"{CasesFolder}original{Path.DirectorySeparatorChar}";
        public static string ExpectedFolder => $"{CasesFolder}expected{Path.DirectorySeparatorChar}";
        public static string ResultsFolder => $"{WorkingDir}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}results{Path.DirectorySeparatorChar}";

    }
}
