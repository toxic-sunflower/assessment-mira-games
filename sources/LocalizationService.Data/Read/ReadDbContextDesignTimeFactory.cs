using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LocalizationService.Data.Read;

public class ReadDbContextDesignTimeFactory : 
    IDesignTimeDbContextFactory<ReadDbContext>
{
    public ReadDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Read");
        var builder = new DbContextOptionsBuilder().UseNpgsql(connectionString);
        
        return new ReadDbContext(builder.Options);
    }
}