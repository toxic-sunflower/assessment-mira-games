using LocalizationService.Domain.Languages;
using LocalizationService.Domain.Translations;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Data.Master;

public class MasterDbContext : DbContext
{
    public DbSet<Language> Languages { get; init; }
    public DbSet<LocalizationKey> LocalizationKeys { get; init; }
    public DbSet<LocalizationKeyTranslation> Translations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDbContext).Assembly);

    public MasterDbContext(DbContextOptions options) : base(options) { }
}