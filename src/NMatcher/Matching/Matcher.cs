namespace NMatcher.Matching
{
    
    internal interface IMatcher
    {
        public Result Match(DynamicValue value);
    }

    internal delegate Result Matches<in T>(T input);
}

