using LocalizationService.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Master.Configurations;

public class LocalizationKeyConfiguration : EntityTypeConfigurationBase<LocalizationKey>
{
    protected override void ConfigureEntity(EntityTypeBuilder<LocalizationKey> builder)
    {
        builder.HasMany(x => x.Translations).WithOne(x => x.LocalizationKey);
        builder.HasIndex(x => x.Key).IsUnique();
    }
}