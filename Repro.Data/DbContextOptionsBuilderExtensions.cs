using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Repro.Data;

public static class DbContextOptionsBuilderExtensions
{

    public static DbContextOptionsBuilder UseNoOpExtension(this DbContextOptionsBuilder builder)
    {
        var ext = builder.Options.FindExtension<NoOpExtension>() ?? new();

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(ext);

        return builder;
    }

}