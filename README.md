# NMatcher [![Build status](https://github.com/defrag/NMatcher/actions/workflows/test.yml/badge.svg?branch=master)](https://ci.appveyor.com/project/MichalDabrowski/nmatcher/branch/master) [![NuGet](https://img.shields.io/nuget/v/NMatcher.svg)](https://www.nuget.org/packages/NMatcher/)

#### NMatcher is a test utility, that lets you easier test responses and json, when some part of the response is something out of your control (autogenerated id, guid, datetime etc). It ports functionality of original php-matcher library to dotnet. 

#### Installation:

```
Install-Package NMatcher
```

### Here is an overview what NMatcher can do for you:

```csharp

[Fact]
public void it_matches_nested_json()
{
    var matcher = new Matcher();

    var result = matcher.MatchJson(
        @"
        {
            ""id"" : ""5a645a20-5225-431b-8c62-031b87f58b73"",
            ""subnode"" : {
                ""city"" : ""NY"",
                ""zipCode"" : ""80-000"",
                ""status"" : ""enabled"",
                ""url"" : ""http://example.com/foo/bar?query=q"",
                ""meta"" : {
                    ""name"" : ""fuuuuuu"",
                    ""shipping"": 99.99,
                    ""enabled"" : false,
                    ""_link"" : ""http://example.com?page=2"",
                    ""_something"" : null,
                    ""_arr"" : [1, 2, 3],
                    ""_arr2"" : [10, 20, 30],
                    ""_date"" : ""2018-01-01""
                }
            },
            ""subnodeArr"": [1, 2, 3, 4, 5, 6] 
        }",
        @"
        {
            ""id"" : ""@guid@"",
            ""subnode"" : {
                ""city"" : ""NY"",
                ""zipCode"" : ""@string@"",
                ""status"" : ""@string@.OneOf('enabled', 'disabled')"",
                ""url"" : ""http://@string@.Contains('example')/foo/@string@?query=@string@"",
                ""meta"" : {
                    ""name"" : ""@string@.Contains('fuu')"",
                    ""shipping"": ""@double@"",
                    ""enabled"" : ""@bool@"",
                    ""_link"" : ""@any@"",
                    ""_something"" : ""@null@"",
                    ""_arr"" : [1, 2, 3],
                    ""_arr2"" : ""@array@"",
                    ""_date"" : ""@string@.IsDateTime()"",
                    ""_signature"" : ""@string?@.Contains('sha')""
                }
            },
            ""subnodeArr"": [1, 2, 3, ""@skip@""]
        }"
    );

    Assert.True(result.Successful);
}
```

#### Skipping a set of following elements while matching json

Sometimes our responses can include a large list of elements. While asserting our protocol of given endpoint,
we may be just interested in general structure assertion of first element, while skipping others.
We can achieve that as follows

```C#
var result = matcher.MatchJson(
    @"
    [
        { "id": "5001", "type": "None" },
        { "id": "5002", "type": "Glazed" },
        { "id": "5005", "type": "Sugar" },
        { "id": "5007", "type": "Powdered Sugar" },
        { "id": "5006", "type": "Chocolate with Sprinkles" },
        { "id": "5003", "type": "Chocolate" },
        { "id": "5004", "type": "Maple" }
    ]",
    @"
    [
        { "id": "@string@", "type": "@string@" },
        ""@skip@""
    ]
    "
);

Assert.True(result.Successful);

```

### Available expressions: 
* @string@
* @int@
* @double@
* @bool@
* @null@
* @any@
* @guid@
* @array@

Expressions come with optional types as well (@string?@, @int?@ etc).


### Basic usage wraps around two methods:

```csharp
using NMatcher();

var matcher = new Matcher();

matcher.MatchExpression("string", "@string@.Contains('str')"); // matching expression
matcher.MatchJson(@"{""enabled"" : true}", @"{""enabled"" : ""@bool@""}"); // matching json

```

### Usage: 

#### String matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression("2018-01-01 11:00:12", "@string@.IsDateTime()");
matcher.MatchExpression("str", "@string@");
matcher.MatchExpression("foobar", "@string@.OneOf('foobar', 'baz')")
matcher.MatchExpression("string", "@string@.Contains('str')");
matcher.MatchExpression(null, "@string?@.Contains('str')"); //optional
```

#### Int matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression(1000, "@int@");
matcher.MatchExpression(11, "@int@.GreaterThan(10)");
matcher.MatchExpression(11, "@int@.LowerThan(100)");
matcher.MatchExpression(11, "@int@.GreaterThan(10).LowerThan(20)");
matcher.MatchExpression(null, "@int?@)"); //optional
```

#### Double matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression(100.00, "@double@");
matcher.MatchExpression(17.59, "@double@.GreaterThan(17.50)");
matcher.MatchExpression(9.5, "@double@.LowerThan(10.0)");
matcher.MatchExpression(null, "@double?@)"); //optional

```

#### Null matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression(null, "@null@")
```

#### Wildcard matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression("string", "@any@");
matcher.MatchExpression(123, "@any@");
matcher.MatchExpression(99.99, "@any@");
matcher.MatchExpression(false, "@any@");
```

#### Guid matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression("843475f5-f7c9-4a28-b028-a3a7dc456e91", "@guid@");
matcher.MatchExpression("C56A4180-65AA-42EC-A945-5FD21DEC0538", "@guid@");
matcher.MatchExpression(null, "@guid?@)"); //optional
```
#### Array matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression(new int[] { 1, 2, 3 }, "@array@");
matcher.MatchExpression(new string[] { "fuu", "bar", "baz" }, "@array@");
```

#### Compound matching:
```csharp
var matcher = new Matcher();
matcher.MatchExpression("https://amazon.com/dp/1SOTO", "https://@string@.Contains(\"amazon\")/dp/@string@")
matcher.MatchExpression("https://amazon.com?isFoo=true", "https://@string@.Contains(\"amazon\")?isFoo=@bool@")
matcher.MatchExpression("https://amazon.com?page=1", "https://@string@.Contains(\"amazon\")?page=@int@")
```

#### JSON matching:
This is where NMatcher shines. Check the first example from README. It allows to combine all expression to achieve easy to use json response matching in your test. All checks can be wrapped with optional condition (eg @string?@), which will ommit assertion when expected node was not found in actual json.

### Integration with test frameworks
NMatcher doesn't come with out of the box integration with test frameworks, but its super easy to roll your own version. Here is a sample with fluent assertions:

```csharp
public static class AssertionsExtensions
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

// use it later on

[Fact]
public async Task it_returns_200_with_product()
{
    var id = Guid.NewGuid();
    await ProductContext.ProductExist(id, "Shampoo", 19.99M);

    var response = await Client.GetAsync($"api/1.0/products/{id}");
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var contents = await response.Content.ReadAsStringAsync();

    contents.Should().MatchJson(@"
        {
          ""Id"": ""@guid@"",
          ""Name"": ""Shampoo"",
          ""Price"": ""19.99""
        }
    ");
}

```

### License

This library is distributed under the MIT license. Please see the LICENSE file.

### Credits

Original [php-matcher](https://github.com/coduo/php-matcher).
