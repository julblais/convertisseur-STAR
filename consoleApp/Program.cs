using STAR.Format;
using System;
using System.Collections.Generic;
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
            List<string> contents = new();

            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open), encoding))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    contents.Add(str);
                }
            }

            var formatter = new Formatter()
                .With(Rules.FixEndline);

            var ctx = new FormattingContext(contents);
            formatter.Format(ctx);

            Console.Write(ctx.ToString());
        }
    }
}