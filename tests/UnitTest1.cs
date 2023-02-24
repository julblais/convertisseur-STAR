using System;
using NUnit.Framework;
using STAR.Format;

namespace STAR.Tests
{
    readonly struct TestCase
    {
        public readonly Formatter.Rule[] rules { get; init; }
        public readonly string fileName { get; init; }

        public TestCase(string fileName, params Formatter.Rule[] rules)
        {
            this.fileName = fileName;
            this.rules = rules;
        }
    }

    class Tests
    {
        static TestCase[] fileCases = new TestCase[]
        {
            new TestCase("testFile.lst", Rules.FixEndline)
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TestCaseSource(nameof(fileCases))]
        public void Test2(TestCase testCase)
        {
            Console.WriteLine(testCase.fileName);
            Console.WriteLine(testCase.rules);
        }
    }
}
