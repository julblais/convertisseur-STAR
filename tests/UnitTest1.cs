using NUnit.Framework;
using STAR.Writer;

namespace STAR.Tests 
{
    public class Tests 
    {
        [SetUp]
        public void Setup() 
        {
        }

        [Test]
        public void Test1() 
        {
            var wordWriter = new WordWriter("test");
            Assert.Pass();
        }
    }
}