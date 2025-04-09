using IntegrationTestPoc.Domain;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestProc.Infra;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    // Defina DbSets para suas entidades
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações adicionais para suas entidades
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
