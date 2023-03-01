using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using STAR.Format;
using STAR.Writer;

namespace STAR.Performance
{
    [HtmlExporter]
    [MemoryDiagnoser]
    public class ConversionBenchmark
    {
        const int CodePage = 28591; //ISO-8859-1 Western European
        static readonly Encoding Encoding = Encoding.GetEncoding(CodePage);

        string m_Data = string.Empty;
        MemoryStream m_MemoryStream;
        StreamWriter m_StreamWriter;
        IDocumentWriter m_Writer;

        [IterationSetup]
        public void IterationSetup()
        {
            m_MemoryStream = new MemoryStream();
            m_StreamWriter = new StreamWriter(m_MemoryStream);
            m_Writer = new TxtWriter();

            //read perf file
            m_Data = ReadFile(InputSource.PerfFolder, InputSource.perfFile);
        }

        [IterationCleanup]
        public void Cleanup()
        {
            m_StreamWriter.Dispose();
            m_MemoryStream.Dispose();
        }

        [Benchmark]
        public void Convert()
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

            IEnumerable<Command> conversion = rules.ApplyTo(m_Data);
        }

        [Benchmark]
        public void Read_And_Convert()
        {
            string contents = ReadFile(InputSource.PerfFolder, InputSource.perfFile);

            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd
            };

            IEnumerable<Command> conversion = rules.ApplyTo(contents);
        }

        [Benchmark]
        public void Convert_And_Save()
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

            IEnumerable<Command> conversion = rules.ApplyTo(m_Data);
            conversion.WriteTo(m_Writer, m_StreamWriter);
        }

        [Benchmark]
        public void Read_And_Convert_And_Save()
        {
            string contents = ReadFile(InputSource.PerfFolder, InputSource.perfFile);

            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd
            };

            IEnumerable<Command> conversion = rules.ApplyTo(m_Data);
            conversion.WriteTo(m_Writer, m_StreamWriter);
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
        public static void Main(string[] _)
        {
            Summary summary = BenchmarkRunner.Run<ConversionBenchmark>();
        }
    }
}
