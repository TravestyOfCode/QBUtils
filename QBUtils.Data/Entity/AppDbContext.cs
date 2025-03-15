using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QBUtils.Data;

internal class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}

internal class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{envName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var connString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Unable to get connection string.");

        var optBuilder = new DbContextOptionsBuilder()
            .UseSqlServer(connString);

        return new AppDbContext(optBuilder.Options);
    }
}
