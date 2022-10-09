namespace NMatcher.Matching.Json
{
    internal sealed record JsonPair(
        object Actual,
        object Expected,
        string Path,
        bool IsEqual,
        JsonPair.ComparisonOrigin Origin = JsonPair.ComparisonOrigin.Scalar)
    {
        public enum ComparisonOrigin
        {
            Scalar,
            Expression
        }
        
        public string ActualAsString =>  Actual is null ? "null" : Actual.ToString();
        public string ActualType =>  Actual is null ? "null" : Actual.GetType().ToString();
        public string ExpectedAsString =>  Expected is null ? "null" : Expected.ToString();
        public string ExpectedType 
        {
            get
            {
                if (Expected is null)
                {
                    return "null";
                }
                
                return Origin == ComparisonOrigin.Scalar ? Expected.GetType().ToString() : "Expression";
            } 
        }
    }
}
