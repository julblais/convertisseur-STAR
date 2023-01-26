using STAR.Format;
using STAR.Writer;
using System;
using System.IO;
using System.Text;

namespace STAR.ConsoleApp
{
    internal class Program
    {
        const string version = "0.1.0";

        static readonly string ArgHelp = "--help";
        static readonly string ArgFile = "--file";
        static readonly string ArgCodePage = "--codepage";
        static readonly string ArgVersion = "--version";

        static string filePath;
        static int codePage = 28591; //ISO-8859-1 Western European
        static bool displayVersion;
        static bool displayHelp;

        static void ParseArguments(string[] args)
        {
            args.Parse(ArgHelp, ref displayHelp);
            args.Parse(ArgFile, ref filePath);
            args.Parse(ArgCodePage, ref codePage);
            args.Parse(ArgVersion, ref displayVersion);
        }

        static void Main(string[] args)
        {
            ParseArguments(args);

            if (displayHelp)
            {
                DisplayHelp();
                return;
            }
            else if (displayVersion)
            {
                DisplayVersion();
                return;
            }

            var encoding = Encoding.GetEncoding(codePage);
            string contents = string.Empty;

            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.FixItalicsStart,
                Rules.FixItalicsEnd
            };

            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), encoding))
            {
                contents = sr.ReadToEnd();
            }

            var commands = rules.ApplyTo(contents);
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            using (StreamWriter writer = new($"{fileName}.{MarkDownWriter.extension}"))
            {
                MarkDownWriter markDownWriter = new();
                commands.WriteTo(markDownWriter, writer);
            }

            using (StreamWriter writer = new($"{fileName}.{WordWriter.extension}"))
            {
                WordWriter wordWriter = new(fileName);
                commands.WriteTo(wordWriter, writer);
            }

            Console.WriteLine("Done!");
        }

        static void DisplayHelp()
        {
            Console.WriteLine(@"This tool can convert STAR files (.lst) to common text file types.

Usage:
    [<> ...] [options]

Arguments:
< file list >

Options:
    --help       Displays help
    --version    Displays version
    --codepage   Changes input codepage (default is ISO-8859-1 Western European)
");
        }

        static void DisplayVersion()
        {
            Console.WriteLine($"{version}");
        }
    }
}