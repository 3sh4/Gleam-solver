using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GleamSolverLibrary.GleamDb;
using GleamSolverLibrary.GleamDb.Parsers;
using NUnit.Framework;

namespace SolverLibTests.GleamDbTests.ParserTests
{
    [TestFixture]
    public class GiveawaysTableParserTests
    {
        public struct ExpectedValues
        {
            public int GiveawaysCount;
            public int HiddenEntriesGiveawaysCount;
        }

        private const string TestDataPathFormat = "SolverLibTests.GleamDbTests.ParserTests.TestData.{0}";

        private const string GoodHtmlResponse = @"GoodResponse.txt";
        private const string EmptyTablesHtmlResponse = @"EmptyTablesResponse.txt";
       
        private static readonly GiveawaysTableParser Parser = new GiveawaysTableParser();

        private static string ReadTestDataHtmlText(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(string.Format(TestDataPathFormat, fileName)))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                return reader.ReadToEnd();
            }
        }

        private static IEnumerable<TestCaseData> GenerateTestCaseData()
        {
            return new[]
            {
                new TestCaseData(
                    Parser.ParseCompetitionsTableFromHtml(ReadTestDataHtmlText(GoodHtmlResponse)),
                    new ExpectedValues {GiveawaysCount = 397, HiddenEntriesGiveawaysCount = 81}),

                new TestCaseData(
                    Parser.ParseCompetitionsTableFromHtml(ReadTestDataHtmlText(EmptyTablesHtmlResponse)),
                    new ExpectedValues {GiveawaysCount = 0, HiddenEntriesGiveawaysCount = 0})
            };
        }

        [TestCase(null)]
        [TestCase("     ")]
        public void NullOrWhitespaceHtmlTest(string html)
        {
            Assert.Throws<ArgumentException>(delegate { Parser.ParseCompetitionsTableFromHtml(html); });
        }

        [TestCaseSource(nameof(GenerateTestCaseData))]
        public void GiveawaysCountTest(List<GleamDbGiveawayInfo> giveaways, ExpectedValues values)
        {
            Assert.AreEqual(giveaways.Count, values.GiveawaysCount);
        }

        [TestCaseSource(nameof(GenerateTestCaseData))]
        public void HiddenEntriesCountTest(List<GleamDbGiveawayInfo> giveaways, ExpectedValues values)
        {
            var hiddenEntriesGiveawaysCount = giveaways.Count(giveaway => giveaway.IsHiddenEntries);
            Assert.AreEqual(hiddenEntriesGiveawaysCount, values.HiddenEntriesGiveawaysCount);
        }

          
    }
}