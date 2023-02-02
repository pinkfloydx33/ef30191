using Xunit;

namespace Repro.Tests.Clones;

public sealed class TestClass6 : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public TestClass6(TestFixture fixture)
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