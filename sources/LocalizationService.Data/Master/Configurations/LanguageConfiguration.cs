using LocalizationService.Domain;
using LocalizationService.Domain.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Master.Configurations;

public class LanguageConfiguration : EntityTypeConfigurationBase<Language>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Language> builder)
    {
        builder.HasIndex(x => x.Locale).IsUnique();
        builder.HasMany(x => x.Translations).WithOne(x => x.Language).OnDelete(DeleteBehavior.NoAction);
    }
}