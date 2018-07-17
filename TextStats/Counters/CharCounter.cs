using System.Collections.Generic;
using Demo.Stats.Reducers;
using Demo.Stats.Selectors;

namespace Demo.Stats.Counters
{
    public class CharCounter
    {
        private ICharReducer reducer;
        private ICharSelector selector;

        public CharCounter(ICharSelector selector, ICharReducer reducer = null) 
        {
            this.selector = selector;
            this.reducer = reducer;
        }

        public Dictionary<char, int> Count(string text)
        {
            var result = new Dictionary<char, int>();

            text = text ?? string.Empty;

            foreach (var character in text)
            {
                if (selector.IsSuitable(character))
                {
                    var invariant = reducer?.Reduce(character) ?? character;

                    if (result.ContainsKey(invariant))
                    {
                        result[invariant]++;
                    }
                    else
                    {
                        result.Add(invariant, 1);
                    }
                }
            }

            return result;
        }
    }
}
