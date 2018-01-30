namespace NMatcher.Matching
{
    public sealed class DoubleMatcher : IMatcher
    {
        public Result Match(object value)
        {
            var res = value is float ||
                value is double ||
                value is decimal;

            return res ? Result.Success() : Result.Failure($"{value} is not a valid double.");
        }
    }
}
