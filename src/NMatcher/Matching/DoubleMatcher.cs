namespace NMatcher.Matching
{
    public sealed class DoubleMatcher : IMatcher
    {
        public bool Match(object value)
        {
            return value is float ||
                value is double ||
                value is decimal;
        }
    }
}
