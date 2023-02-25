using NUnit.Framework;
using STAR.Format;

namespace STAR.Tests
{
    class Generator
    {
        [TestCase(InputSource.b_File)]
        [TestCase(InputSource.d_File)]
        public void GeneratorExpected(string file)
        {
            FileUtilities.ReadAndConvertAndSave(
                InputSource.originalFolder, InputSource.expectedFolder, file,
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd);
        }
    }
}
