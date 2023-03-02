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
            FileUtilities.ReadAndConvertAndSave(
                InputSource.OriginalFolder, InputSource.ExpectedFolder,
                inputFile, outputFile, documentWriter,
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
            FileUtilities.ReadAndConvertAndSave(
                InputSource.OriginalFolder, InputSource.ExpectedFolder,
                inputFile, outputFile, documentWriter,
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd);
        }
    }
}
