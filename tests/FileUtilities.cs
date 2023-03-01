using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STAR.Format;

namespace STAR.Tests
{
    public static class FileUtilities
    {
        const int Seed = 0x123456;
        const int CodePage = 28591; //ISO-8859-1 Western European
        public static readonly Encoding Encoding = Encoding.GetEncoding(CodePage);

        public static string ReadFile(string folder, string fileName)
        {
            string path = folder + fileName;
            using var sr = new StreamReader(File.Open(path, FileMode.Open), Encoding);
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

            using StreamWriter wr = new(outputPath, false, Encoding);
            commands.WriteTo(documentWriter, wr);
        }

        public static void SaveFile(string content, string folder, string fileName)
        {
            SaveFile(content, folder, fileName, Encoding);
        }

        public static void SaveFile(string content, string folder, string fileName, Encoding encoding)
        {
            string outputPath = folder + fileName;
            using StreamWriter wr = new(outputPath, false, encoding);
            wr.Write(content);
        }

        public static string RandomizeLetters(string input)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(Seed);

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
