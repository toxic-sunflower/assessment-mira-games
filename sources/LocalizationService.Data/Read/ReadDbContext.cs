using LocalizationService.Data.Read.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Data.Read;

public class ReadDbContext : DbContext
{
    public DbSet<Translation> Translations { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDbContext).Assembly);
    
    public ReadDbContext(DbContextOptions options) : base(options) { }
}