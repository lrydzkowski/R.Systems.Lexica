using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Lexica.Infrastructure.Db.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.Common.Configurations;

internal class TranslationConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureRelations(builder);
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

    private void ConfigureRelations(EntityTypeBuilder<TranslationEntity> builder)
    {
        ConfigureRelationWithWord(builder);
    }

    private static void ConfigureRelationWithWord(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.HasOne(entity => entity.Word)
            .WithMany(parent => parent.Translations)
            .HasForeignKey(entity => entity.WordId)
            .OnDelete(DeleteBehavior.Cascade);
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

        builder.Property(entity => entity.Order)
            .HasColumnName("order")
            .IsRequired();

        builder.Property(entity => entity.WordId)
            .HasColumnName("word_id")
            .IsRequired();
    }
}
