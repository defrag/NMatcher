namespace NMatcher.Matching
{
    
    public interface IMatcher
    {
        public Result Match(DynamicValue value);
    }

    public delegate Result Matches<in T>(T input);
}

