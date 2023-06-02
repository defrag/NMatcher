using Microsoft.AspNetCore.Mvc.Testing;

namespace NMatcher.Samples.Api.Tests;

sealed class TestApiProgram : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
    }
}