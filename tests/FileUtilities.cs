using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STAR.Format;
using STAR.Writer;

namespace STAR.Tests
{
    public static class FileUtilities
    {
        const int Seed = 0x123456;
        const int CodePage = 28591; //ISO-8859-1 Western European
        public static readonly Encoding Encoding = Encoding.GetEncoding(CodePage);

        public static string ReadFile(string folder, string fileName, Encoding encoding)
        {
            string path = folder + fileName;
            using var sr = new StreamReader(File.Open(path, FileMode.Open), encoding);
            return sr.ReadToEnd();
        }

        public static string ReadFile(string folder, string fileName)
        {
            string path = folder + fileName;
            using var sr = new StreamReader(File.Open(path, FileMode.Open));
            return sr.ReadToEnd();
        }

        public static void ConvertAndSave(string content, string folder, string fileName, IDocumentWriter writer, params Formatter.Rule[] rules)
        {
            IEnumerable<Command> commands = rules.ApplyTo(content);

            string outputPath = folder + fileName;

            using StreamWriter wr = new(outputPath, false);
            commands.WriteTo(writer, wr);
        }

        public static void ConvertAndSave(string content, string folder, string fileName, IDocumentWriter writer, Encoding encoding, params Formatter.Rule[] rules)
        {
            IEnumerable<Command> commands = rules.ApplyTo(content);

            string outputPath = folder + fileName;

            using StreamWriter wr = new(outputPath, false, encoding);
            commands.WriteTo(writer, wr);
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
