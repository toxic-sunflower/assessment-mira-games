using LocalizationService.Data.Read.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Read.Configurations;

public class PlainTranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.HasNoKey();
        builder.HasIndex(nameof(Translation.Key), nameof(Translation.Locale)).IsUnique();
        builder.HasIndex(x => x.Key);
        builder.HasIndex(x => x.Locale);
    }
}