using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LocalizationService.Data.Master;

public class MasterDbContextDesignTimeFactory : 
    IDesignTimeDbContextFactory<MasterDbContext>
{
    public MasterDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Master");
        var builder = new DbContextOptionsBuilder().UseNpgsql(connectionString);
        
        return new MasterDbContext(builder.Options);
    }
}