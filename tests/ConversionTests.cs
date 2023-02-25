using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiffMatchPatch;
using NUnit.Framework;
using STAR.Format;

namespace STAR.Tests
{
    class ConversionTests
    {
        static readonly Formatter.Rule[] Rules = new Formatter.Rule[]
        {
            Format.Rules.FixEndline,
            Format.Rules.FixStartRecord,
            Format.Rules.AddRecordSections,
            //Format.Rules.FixLongSpaces,
            Format.Rules.RemoveItalicsStart,
            Format.Rules.RemoveItalicsEnd
        };

        [TestCase(InputSource.b_File)]
        [TestCase(InputSource.d_File)]
        public void Conversion_ReturnsExpectedResults(string file)
        {
            string input = ReadOriginalFile(file);
            string expected = ReadExpectedFile(file);

            string actual = Convert(input);
            List<Diff> diff = Diff(actual, expected);
            string html = ConvertDiffToHTML(diff);

            bool hasNoDifference = diff.Count == 1 && diff[0].operation == Operation.EQUAL;
            if (hasNoDifference)
            {
                Assert.Pass();
                return;
            }

            WriteActual($"{file}.actual.txt", actual);
            WriteResultFail($"{file}.html", html);

            int count = diff.Count;
            Assert.Fail($"Conversion does not give expected results. Differences count {count}.");
        }

        static string Convert(string input)
        {
            IEnumerable<Command> commands = Rules.ApplyTo(input);

            var rawWriter = new RawWriter();
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, FileUtilities.encoding);
            commands.WriteTo(rawWriter, writer);

            writer.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memoryStream, FileUtilities.encoding);
            return streamReader.ReadToEnd();
        }

        static string ReadOriginalFile(string file) => FileUtilities.ReadFile(InputSource.OriginalFolder, file);
        static string ReadExpectedFile(string file) => FileUtilities.ReadFile(InputSource.ExpectedFolder, file);

        static void WriteActual(string fileName, string actual)
        {
            Console.WriteLine($"Writing actual file at path {InputSource.ResultsFolder}{fileName}");
            FileUtilities.SaveFile(actual, InputSource.ResultsFolder, fileName);
        }

        static void WriteResultFail(string fileName, string diff)
        {
            Console.WriteLine($"Writing actual file at path {InputSource.ResultsFolder}{fileName}");
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
