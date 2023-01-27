using STAR.Format;
using STAR.Writer;
using System;
using System.IO;
using System.Text;

namespace STAR.ConsoleApp
{
    internal class Program
    {
        const string version = "0.5.2";

        const string ArgHelp = "--help";
        const string ArgCodePage = "--codepage";
        const string ArgVersion = "--version";
        const string ArgOutputFormat = "--format";

        const string WordStringFormat = "word";
        const string MarkdownStringFormat = "markdown";

        static string[] filePaths;
        static string outputFormat = WordStringFormat;
        static int codePage = 28591; //ISO-8859-1 Western European
        static bool displayVersion;
        static bool displayHelp;

        static void ParseArguments(string[] args)
        {
            args.Parse(ArgHelp, ref displayHelp);
            args.Parse(ArgVersion, ref displayVersion);
            args.Parse(ArgCodePage, ref codePage);
            args.Parse(ArgOutputFormat, ref outputFormat);
            filePaths = args.ParseArgsList();
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
            else if (filePaths.Length <= 0)
            {
                Console.WriteLine("No files to convert.");
                return;
            }

            foreach(var filePath in filePaths)
                ConvertFile(filePath, outputFormat);

            Console.WriteLine();
            Console.WriteLine("Done!");
        }

        static void ConvertFile(string filePath, string format)
        {
            var encoding = Encoding.GetEncoding(codePage);
            string contents = string.Empty;

            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                //Rules.FixItalicsStart,
                //Rules.FixItalicsEnd
            };

            Console.WriteLine($"Reading file: {filePath}");
            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), encoding))
            {
                contents = sr.ReadToEnd();
            }

            var commands = rules.ApplyTo(contents);
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var documentWriter = CreateDocumentWriter(fileName, format);
            var outputPath = $"{fileName}.{documentWriter.extension}";

            Console.WriteLine($"Writing to file: {outputPath}");
            using (StreamWriter wr = new(outputPath))
            {
                commands.WriteTo(documentWriter, wr);
            }

            Console.WriteLine("Finished conversion");
            Console.WriteLine();
        }

        static IDocumentWriter CreateDocumentWriter(string fileName, string format)
        {
            if (format == WordStringFormat)
                return new WordWriter(fileName);
            else if (format == MarkdownStringFormat)
                return new MarkDownWriter();
            return null;
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
    --format     Defines output format (word|markdown). Default is word.
    --codepage   Changes input codepage. Default is ISO-8859-1 Western European.
");
        }

        static void DisplayVersion()
        {
            Console.WriteLine($"{version}");
        }
    }
}