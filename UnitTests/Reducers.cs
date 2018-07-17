using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Stats.Reducers;

namespace UnitTests
{
    [TestClass]
    public class Reducers
    {
        [TestMethod]
        public void Reducers_Lowercase_Reducer_Should_Lowercase()
        {
            var reducer = new LowerCaseReducer();

            string result = string.Empty;

            foreach(var character in TextStatsTest.uppercaseAlphabet)
            {
                result += reducer.Reduce(character).ToString();
            }

            Assert.AreEqual(result, TextStatsTest.lowercaseAlphabet);
        }
    }
}
