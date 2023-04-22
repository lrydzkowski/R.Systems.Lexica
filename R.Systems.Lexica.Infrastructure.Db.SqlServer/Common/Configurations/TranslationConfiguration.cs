using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class TranslationConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.ToTable(name: "translation");
    }

    private void SetPrimaryKey(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.HasKey(entity => entity.TranslationId);
    }

    private void ConfigureColumns(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.Property(entity => entity.TranslationId)
            .HasColumnName("translation_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(entity => entity.Translation)
            .HasColumnName("translation")
            .IsRequired()
            .HasMaxLength(200);
    }
}
