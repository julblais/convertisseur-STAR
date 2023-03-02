using NUnit.Framework;

namespace STAR.Tests
{
    [Ignore("Used to generate input files")]
    class RandomizeInput
    {
        [TestCase(Input.B_File)]
        [TestCase(Input.D_File)]
        public void Randomize(string file)
        {
            string contents = FileUtilities.ReadFile(InputSource.OriginalFolder, file);
            string randomized = FileUtilities.RandomizeLetters(contents);
            FileUtilities.SaveFile(randomized, InputSource.OriginalFolder, file);
        }
    }
}
