using Microsoft.EntityFrameworkCore;

namespace currency_rate
{
    public class CurrencyRateContext : DbContext
    {
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "6247";
            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database=Currency;Username={user};Password={pass}");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rate>().HasKey(e => new { e.CurrencyID, e.Date });
        }
    }
}
