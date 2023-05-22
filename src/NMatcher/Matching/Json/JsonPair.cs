#nullable enable
namespace NMatcher.Matching.Json
{
    internal sealed record JsonPair(
        DynamicValue? Actual,
        DynamicValue? Expected,
        string Path,
        bool IsEqual,
        JsonPair.ComparisonOrigin Origin = JsonPair.ComparisonOrigin.Scalar)
    {
        public enum ComparisonOrigin
        {
            Scalar,
            Expression
        }
        
        public string ActualAsString => 
            Actual is null ? "null" : Actual.ToString();

        public string ExpectedAsString
        {
            get
            {
                if (Expected is null)
                {
                    return "null";
                }
                
                return Origin == ComparisonOrigin.Scalar 
                    ? Expected.ToString() 
                    : $"\"{Expected.Value}\" (Expression)";
            }
        }
    }
}
