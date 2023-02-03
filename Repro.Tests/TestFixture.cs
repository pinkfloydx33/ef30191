using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Repro.Data;
using Xunit;

namespace Repro.Tests;

public class TestFixture: IAsyncLifetime
{
    private readonly string _connString;
    private readonly DbDataSource _dataSource;

    public TestFixture()
    {
        // Set your connection string!!!!!!!
        // In our actual scenario test scenario, each TestFixture/TestClass runs against a dedicated 
        // docker container (server) so it's necessary to create multiple DbDataSources. For normal 
        // execution of our application we only use one	
        // To mimic the docker-scenario, we use a different database per test fixture
        _connString = "Host=127.0.0.1;Port=5432;UserName=postgres;Password=postgres;Database=Test" + Guid.NewGuid().ToString("N");
        _dataSource = new NpgsqlDataSourceBuilder(_connString).Build();
    }


    // or cache the options... doesn't matter
    public SampleContext CreateContext()
        => new SampleContext(CreateOptions()); 

    
    private DbContextOptions<SampleContext> CreateOptions()
    {
        var options = new DbContextOptionsBuilder<SampleContext>();

        // variant 1 (DbDataSource): throws error
        options.UseNpgsql(_dataSource); // <--- We are NOT re-adding any of the extensions added by context

        // // variant 2 (DbDataSource): no error
        // options.UseNpgsql(_dataSource)
        //     .UseNoOpExtension(); // <--- Need to re-add this even though it's added by Context! 
        //                          //      (must include *all* extensions added by context)

        // // variant 3 (ConnectionString): no error
        // options.UseNpgsql(_connString);

        // // variant 4 (ConnectionString): no error
        // options.UseNpgsql(_connString)
        //     .UseNoOpExtension();


        return options.Options;
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        try
        {
            await using var ctx = CreateContext();
            await ctx.Database.EnsureDeletedAsync();
        }
        catch
        {
            // ok
        }
    }
}
