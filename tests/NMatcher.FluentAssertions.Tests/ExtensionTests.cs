using FluentAssertions;

namespace NMatcher.FluentAssertions.Tests;

public class ExtensionTests
{
    [Fact]
    public void test_that_extension_is_working_on_string()
    {
        @"{""foo"": ""bar""}".Should().MatchJson(@"{""foo"": ""bar""}");
    }
}