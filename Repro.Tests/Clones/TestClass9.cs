using Xunit;

namespace Repro.Tests.Clones;

public sealed class TestClass9 : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public TestClass9(TestFixture fixture)
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