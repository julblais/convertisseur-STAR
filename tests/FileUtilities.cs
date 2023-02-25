using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using STAR.Format;

namespace STAR.Tests
{
    class BasicTestsSource
    {
        const string originalFolder = "../../../cases/original/";
        const string expectedFolder = "../../../cases/expected/";

        public const string b_File = "b.txt";
        public const string d_File = "d.txt";
        /*
                [TestCase(b_File)]
                public void Generate_AddRecordSection(string file)
                {
                    FilesUtilities.ReadAndConvertAndSave(originalFolder, expectedFolder, file,
                        Rules.FixEndline,
                        Rules.FixStartRecord,
                        Rules.AddRecordSections,
                        Rules.FixLongSpaces,
                        Rules.RemoveItalicsStart,
                        Rules.RemoveItalicsEnd);
                }*/

        [TestCase(b_File)]
        public void Randomize(string file)
        {
            string contents = FilesUtilities.ReadFile(originalFolder, file);
            string randomized = FilesUtilities.RandomizeLetters(contents);
            FilesUtilities.SaveFile(randomized, originalFolder, file);
        }
    }

    public static class FilesUtilities
    {
        const int seed = 0x123456;
        const int codePage = 28591; //ISO-8859-1 Western European
        static readonly Encoding encoding = Encoding.GetEncoding(codePage);

        public static string ReadFile(string folder, string fileName)
        {
            string path = folder + fileName;
            using var sr = new StreamReader(File.Open(path, FileMode.Open), encoding);
            return sr.ReadToEnd();
        }

        public static void ReadAndConvertAndSave(string sourceFolder, string outputFolder, string fileName, params Formatter.Rule[] rules)
        {
            string contents = ReadFile(sourceFolder, fileName);
            ConvertAndSave(contents, outputFolder, fileName, rules);
        }

        public static void ConvertAndSave(string content, string folder, string fileName, params Formatter.Rule[] rules)
        {
            IEnumerable<Command> commands = rules.ApplyTo(content);

            var documentWriter = new RawWriter();
            string outputPath = folder + fileName;

            using StreamWriter wr = new(outputPath, false, encoding);
            commands.WriteTo(documentWriter, wr);
        }

        public static void SaveFile(string content, string folder, string fileName)
        {
            string outputPath = folder + fileName;
            using StreamWriter wr = new(outputPath, false, encoding);
            wr.Write(content);
        }

        public static string RandomizeLetters(string input)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(seed);

            var builder = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                char output = c;
                if (allowedChars.Contains(c)) //should replace char with random value
                    output = allowedChars[random.Next(0, allowedChars.Length)];
                builder.Append(output);
            }

            return builder.ToString();
        }
    }
}
