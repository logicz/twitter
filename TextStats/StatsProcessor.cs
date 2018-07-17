using Demo.Stats.Counters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Stats
{
    public class StatsProcessor
    {
        private CharCounter counter;

        private Dictionary<char, int> charsCounts = new Dictionary<char, int>();

        public StatsProcessor(CharCounter counter)
        {
            this.counter = counter;
        }

        public void Clear()
        {
            charsCounts = new Dictionary<char, int>();
        }

        public void Add(string text)
        {
            Merge(counter.Count(text));
        }

        public Dictionary<char, double> GetFrequence(int dights = 5)
        {
            var result = new Dictionary<char, double>();

            var sum = charsCounts.Sum(count => (double)count.Value);

            // todo: exclude sorting stratagy

            var list = charsCounts.Keys.ToList();
            list.Sort();

            foreach (var key in list)
            {
                result.Add(key, Math.Round(charsCounts[key] / sum, dights));
            }

            return result;
        }

        public void Merge(Dictionary<char, int> counts)
        {
            foreach(var count in counts)
            {
                if(charsCounts.ContainsKey(count.Key))
                {
                    charsCounts[count.Key] += count.Value;
                }
                else
                {
                    charsCounts[count.Key] = count.Value;
                }
            }
        }
    }
}
