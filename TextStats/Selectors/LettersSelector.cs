namespace Demo.Stats.Selectors
{
    class LettersSelector : ICharSelector
    {
        public bool IsSuitable(char character)
        {
            return char.IsLetter(character);
        }
    }
}
