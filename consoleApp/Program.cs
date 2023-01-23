using STAR.Format;
using STAR.Writer;
using System;
using System.IO;
using System.Text;

namespace STAR.ConsoleApp
{
    internal class Program
    {
        static readonly string ArgFile = "--file";
        static readonly string ArgCodePage = "--codepage";

        static string filePath;
        static int codePage = 28591; //iso-8859-1 Western European

        static void ParseArguments(string[] args)
        {
            args.Parse(ArgFile, ref filePath);
        }

        static void Main(string[] args)
        {
            ParseArguments(args);

            var encoding = Encoding.GetEncoding(codePage);
            string contents = string.Empty;


            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline
            };

            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), encoding))
            {
                contents = sr.ReadToEnd();
            }

            var commands = rules.ApplyTo(contents);

            using (StreamWriter writer = new("output" + MarkDownWriter.extension))
            {
                commands.WriteTo(MarkDownWriter.WriteCommands, writer);
            }

            commands.WriteTo(MarkDownWriter.WriteCommands, Console.Out);
        }
    }
}