using NUnit.Framework;
using STAR.Format;

namespace STAR.Tests
{
    [Ignore("Used to generate input files")]
    class Generator
    {
        [TestCase(Input.B_File)]
        [TestCase(Input.D_File)]
        public void GeneratorExpected(string file)
        {
            FileUtilities.ReadAndConvertAndSave(
                InputSource.OriginalFolder, InputSource.ExpectedFolder, file,
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd);
        }
    }
}
