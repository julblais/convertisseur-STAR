using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiffMatchPatch;
using NUnit.Framework;
using STAR.Format;
using STAR.Writer;

namespace STAR.Tests
{
    public static class Input
    {
        public const string B_File = "b.txt";
        public const string D_File = "d.txt";
    }

    public static class Expected
    {
        public static class Raw
        {
            public const string B_File = "b_Raw.txt";
            public const string D_File = "d_Raw.txt";
        }

        public static class Word
        {
            public const string B_File = "b_Word.txt";
            public const string D_File = "d_Word.txt";
        }
    }

    class ConversionTests
    {
        static readonly Formatter.Rule[] Rules = new Formatter.Rule[]
        {
            Format.Rules.FixEndline,
            Format.Rules.FixStartRecord,
            Format.Rules.AddRecordSections,
            Format.Rules.FixLongSpaces,
            Format.Rules.RemoveItalicsStart,
            Format.Rules.RemoveItalicsEnd
        };

        [TestCase(Input.B_File, Expected.Raw.B_File)]
        [TestCase(Input.D_File, Expected.Raw.D_File)]
        public void Conversion_Raw_ReturnsExpectedResults(string inputFile, string expectedFile)
        {
            string input = ReadOriginalFile(inputFile);
            string expected = ReadExpectedFile(expectedFile);

            string actual = Convert(new RawWriter(), input);
            List<Diff> diff = Diff(actual, expected);
            string html = ConvertDiffToHTML(diff);

            bool hasNoDifference = diff.Count == 1 && diff[0].operation == Operation.EQUAL;
            if (hasNoDifference)
            {
                Assert.Pass();
                return;
            }

            WriteActual($"{inputFile}.actual.txt", actual);
            WriteResultFail($"{inputFile}.html", html);

            int count = diff.Count;
            Assert.Fail($"Conversion does not give expected results. Differences count {count}.");
        }
        /*
        [TestCase(Input.B_File)]
        [TestCase(Input.D_File)]
        public void Conversion_Word_ReturnsExpectedResults(string file)
        {
            string input = ReadOriginalFile(file);
            string expected = ReadExpectedFile(file);

            string actual = Convert(new WordWriter(file), input);
            List<Diff> diff = Diff(actual, expected);
            string html = ConvertDiffToHTML(diff);

            bool hasNoDifference = diff.Count == 1 && diff[0].operation == Operation.EQUAL;
            if (hasNoDifference)
            {
                Assert.Pass();
                return;
            }

            WriteActual($"{file}.actual.doc", actual);
            WriteResultFail($"{file}.html", html);

            int count = diff.Count;
            Assert.Fail($"Conversion does not give expected results. Differences count {count}.");
        }*/

        static string Convert(IDocumentWriter documentWriter, string input)
        {
            IEnumerable<Command> commands = Rules.ApplyTo(input);

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, FileUtilities.Encoding);
            commands.WriteTo(documentWriter, writer);

            writer.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memoryStream, FileUtilities.Encoding);
            return streamReader.ReadToEnd();
        }

        static string ReadOriginalFile(string file) => FileUtilities.ReadFile(InputSource.OriginalFolder, file);
        static string ReadExpectedFile(string file) => FileUtilities.ReadFile(InputSource.ExpectedFolder, file);

        static void WriteActual(string fileName, string actual)
        {
            Console.WriteLine($"Writing actual file: {InputSource.ResultsFolder}{fileName}");
            FileUtilities.SaveFile(actual, InputSource.ResultsFolder, fileName);
        }

        static void WriteResultFail(string fileName, string diff)
        {
            Console.WriteLine($"Writing results file: {InputSource.ResultsFolder}{fileName}");
            FileUtilities.SaveFile(diff, InputSource.ResultsFolder, fileName, Encoding.UTF8);
        }

        static List<Diff> Diff(string actual, string expected)
        {
            var differ = new diff_match_patch();
            List<Diff> diff = differ.diff_main(expected, actual);
            differ.diff_cleanupEfficiency(diff);
            return diff;
        }

        static string ConvertDiffToHTML(List<Diff> diffs) => diff_match_patch.diff_prettyHtml(diffs);
    }
}
