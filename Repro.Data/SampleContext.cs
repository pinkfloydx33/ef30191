using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repro.Data;

public class SampleContext : DbContext
{
    public DbSet<Person> People => Set<Person>();

    public SampleContext()
    {
    }

    public SampleContext(DbContextOptions<SampleContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;User=postgres;Password=postgres;Database=sample");
        
        if (!optionsBuilder.Options.IsFrozen)  // <---- Adding the extension here
            optionsBuilder.UseNoOpExtension(); // ALL extensions added here MUST be re-added in Test setup
                                               // otherwise we will experience ManyServiceProvidersCreatedWarning

    }

}

[Table("people")]
public class Person
{
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public required string FirstName { get; set; }
    [Column("last_name")]
    public required string LastName { get; set; }

    [Column("preferred_name")]
    public string? PreferredName { get; set; }
}