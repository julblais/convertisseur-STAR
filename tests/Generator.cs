using NUnit.Framework;
using STAR.Format;
using STAR.Writer;

namespace STAR.Tests
{
    [Ignore("Used to generate input files")]
    class Generator
    {
        [TestCase(Input.B_File, Expected.Raw.B_File)]
        [TestCase(Input.D_File, Expected.Raw.D_File)]
        public void GeneratorExpectedRaw(string inputFile, string outputFile)
        {
            var documentWriter = new RawWriter();
            string contents = FileUtilities.ReadFile(InputSource.OriginalFolder, inputFile);

            FileUtilities.ConvertAndSave(contents, InputSource.ExpectedFolder, outputFile,
                documentWriter, FileUtilities.Encoding,
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd);
        }

        [TestCase(Input.B_File, Expected.Word.B_File)]
        [TestCase(Input.D_File, Expected.Word.D_File)]
        public void GeneratorExpectedWord(string inputFile, string outputFile)
        {
            var documentWriter = new WordWriter(outputFile);
            string contents = FileUtilities.ReadFile(InputSource.OriginalFolder, inputFile);

            FileUtilities.ConvertAndSave(contents, InputSource.ExpectedFolder, outputFile, documentWriter,
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd);
        }
    }
}
