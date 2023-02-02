using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Repro.Data;

public sealed class NoOpExtension : IDbContextOptionsExtension
{
    public void ApplyServices(IServiceCollection services)
    {
        // empty
    }

    public void Validate(IDbContextOptions options)
    {
        // empty
    }

    private DbContextOptionsExtensionInfo? _info;

    public DbContextOptionsExtensionInfo Info  => _info ??= new ExtensionInfo(this);

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }
        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            => debugInfo["NoOpExtension"] = "Added!";

        public override bool IsDatabaseProvider => false;

        private string? _fragment;
        public override string LogFragment =>
            _fragment ??= "NoOpExtension";
    }
}