using System.Text.Json;
using LocalizationService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizationService.Data.Master.Configurations;

public class ChangeConfiguration : IEntityTypeConfiguration<Change>
{
    public void Configure(EntityTypeBuilder<Change> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.CurrentState)
            .HasColumnType("jsonb")
            .HasConversion(
                x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                x => JsonSerializer.Deserialize<LocalizationKeyState>(x, JsonSerializerOptions.Default));
        
        builder.Property(x => x.NewState)
            .HasColumnType("jsonb")
            .HasConversion(
                x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                x => JsonSerializer.Deserialize<LocalizationKeyState>(x, JsonSerializerOptions.Default));
        
        builder.HasIndex(x => x.Key).IsUnique();
    }
}