using System;

namespace Demo.Stats.Reducers
{
    public class LowerCaseReducer : ICharReducer
    {
        public char Reduce(char letter)
        {
            return Char.ToLowerInvariant(letter);
        }
    }
}
