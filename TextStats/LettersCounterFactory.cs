using Demo.Stats.Counters;
using Demo.Stats.Reducers;
using Demo.Stats.Selectors;

namespace Demo.Stats
{
    public class LettersCounterFactory : CountersFactory
    {
        public override CharCounter GetCounter()
        {
            return new CharCounter(new LettersSelector(), new LowerCaseReducer());
        }
    }
}
