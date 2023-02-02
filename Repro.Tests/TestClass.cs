using Xunit;

namespace Repro.Tests;

public sealed class TestClass : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public TestClass(TestFixture fixture)
        => _fixture = fixture;

    [Fact]
    public async Task Test()
    {
        await using var ctx = _fixture.CreateContext();
        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.EnsureCreatedAsync();
        
        Assert.True(true);
    }
}