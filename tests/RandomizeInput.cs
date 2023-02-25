using NUnit.Framework;

namespace STAR.Tests
{
    [Ignore("Used to generate input files")]
    class RandomizeInput
    {
        [TestCase(InputSource.b_File)]
        [TestCase(InputSource.d_File)]
        public void Randomize(string file)
        {
            string contents = FileUtilities.ReadFile(InputSource.originalFolder, file);
            string randomized = FileUtilities.RandomizeLetters(contents);
            FileUtilities.SaveFile(randomized, InputSource.originalFolder, file);
        }
    }
}
