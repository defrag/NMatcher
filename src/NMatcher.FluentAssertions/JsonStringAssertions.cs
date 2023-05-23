using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace NMatcher.FluentAssertions;

public static class JsonStringAssertionsExtensions
{
    public static AndConstraint<StringAssertions> MatchJson(this StringAssertions assertions, string expected, string because = "", params object[] becauseArgs)
    {
        var matcher = new Matcher();
        var result = matcher.MatchJson(assertions.Subject, expected);
        Execute.Assertion
            .ForCondition(result.Successful)
            .BecauseOf(because, becauseArgs)
            .FailWith($"Json matching failed because of following reason: '{result.ErrorMessage}'.");
        
        return new AndConstraint<StringAssertions>(assertions);
    }
}