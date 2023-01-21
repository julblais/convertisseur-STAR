using STAR.Format;
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
            StringBuilder contents = null;

            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), encoding))
            {
                contents = new StringBuilder(sr.ReadToEnd());
            }

            Console.Write(contents.ToString());

            var formatter = new Formatter()
                .With(Rules.RemoveItalics);
        }

        static void Format()
        {

        }
    }
}