using LocalizationService.Data.Read.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Read.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.HasNoKey();

        builder.Property(x => x.Identifier);
        
        builder.HasIndex(x => x.Identifier).IsUnique();
        builder.HasIndex(x => x.LocalizationKey).IsUnique();
        builder.HasIndex(x => x.Locale).IsUnique();
    }
}