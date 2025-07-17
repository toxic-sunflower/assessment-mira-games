using LocalizationService.Domain;
using LocalizationService.Domain.Translations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Master.Configurations;

public class LocalizationKeyTranslationConfiguration : EntityTypeConfigurationBase<LocalizationKeyTranslation>
{
    protected override void ConfigureEntity(EntityTypeBuilder<LocalizationKeyTranslation> builder)
    {
        builder.HasOne(x => x.Language).WithMany(x => x.Translations);
    }
}