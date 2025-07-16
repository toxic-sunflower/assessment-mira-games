using LocalizationService.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Master.Configurations;

public class LanguageConfiguration : EntityTypeConfigurationBase<Language>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Language> builder)
    {
        builder.HasIndex(x => x.Code).IsUnique();
    }
}