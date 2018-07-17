using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Stats;
using System.Linq;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class TextStatsTest
    {
        public const string uppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string lowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private string dights = "0123456789";
        private string multiLanguage = "abcабв神農本פגשהجِاهِلِ ";

        private CountersFactory StatsCounterFactory = new LettersCounterFactory();

        [TestMethod]
        public void LettersCounter_Should_Input_NullValues_Correctly()
        {
            var counter = StatsCounterFactory.GetCounter();
            counter.Count(null);
        }
        
        [TestMethod]
        public void LettersCounter_Should_Input_EmptyValues_Correctly()
        {
            var counter = StatsCounterFactory.GetCounter();
            counter.Count(string.Empty);
        }

        private void AssertResultsForEnglishAlphabet(Dictionary<Char, int> stats, int count = 1)
        {
            Assert.AreEqual(stats.Count(), 26);
            Assert.AreEqual(stats.Where(r => r.Value != count).Count(), 0);
        }

        [TestMethod]
        public void LettersCounter_Should_Count_LatinUppercase()
        {
            var counter = StatsCounterFactory.GetCounter();
            var result = counter.Count(uppercaseAlphabet);

            AssertResultsForEnglishAlphabet(result);
        }

        [TestMethod]
        public void LettersCounter_Should_Count_LatinLowercase()
        {
            var counter = StatsCounterFactory.GetCounter();
            var result = counter.Count(lowercaseAlphabet);

            AssertResultsForEnglishAlphabet(result);
        }

        [TestMethod]
        public void LettersCounter_Should_Count_CaseInsencitive()
        {
            var counter = StatsCounterFactory.GetCounter();
            var result = counter.Count(lowercaseAlphabet + uppercaseAlphabet);

            AssertResultsForEnglishAlphabet(result, 2);
        }

        [TestMethod]
        public void LettersCounter_Should_Not_Count_Dights()
        {
            var counter = StatsCounterFactory.GetCounter();
            var result = counter.Count(dights);

            Assert.AreEqual(result.Count(), 0);
        }
        
        [TestMethod]
        public void LettersCounter_Should_Count_Different_Letters_MultiLanguage()
        {
            var counter = StatsCounterFactory.GetCounter();
            var result = counter.Count(multiLanguage);

            Assert.AreEqual(result.Count(), 17);

            foreach (var r in result)
            {
                Assert.AreEqual(r.Value, 1);
            }
        }
    }
}
