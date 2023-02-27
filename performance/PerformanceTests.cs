using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dia2Lib;
using STAR.Format;

namespace STAR.Performance
{
    [MemoryDiagnoser]
    public class Md5VsSha256
    {
        const int CodePage = 28591; //ISO-8859-1 Western European
        static readonly Encoding Encoding = Encoding.GetEncoding(CodePage);

        string m_Data = string.Empty;

        [IterationSetup]
        public void IterationSetup()
        {
            //read perf file
            m_Data = ReadFile(InputSource.PerfFolder, InputSource.perfFile);
        }

        [Benchmark]
        public void ConvertFile()
        {
            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd
            };

            return rules.ApplyTo(contents);
        }

        static string ReadFile(string folder, string fileName)
        {
            string path = folder + fileName;
            using var sr = new StreamReader(File.Open(path, FileMode.Open), Encoding);
            return sr.ReadToEnd();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Md5VsSha256>();
        }
    }
}
