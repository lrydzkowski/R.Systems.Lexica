﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class WordConfiguration : IEntityTypeConfiguration<WordEntity>
{
    public void Configure(EntityTypeBuilder<WordEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureRelations(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<WordEntity> builder)
    {
        builder.ToTable(name: "word");
    }

    private void SetPrimaryKey(EntityTypeBuilder<WordEntity> builder)
    {
        builder.HasKey(entity => entity.WordId);
    }

    private void ConfigureRelations(EntityTypeBuilder<WordEntity> builder)
    {
        ConfigureRelationWithWordType(builder);
        ConfigureRelationWithTranslations(builder);
    }

    private static void ConfigureRelationWithWordType(EntityTypeBuilder<WordEntity> builder)
    {
        builder.HasOne(entity => entity.WordType)
            .WithMany(parent => parent.Words)
            .HasForeignKey(entity => entity.WordTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureRelationWithTranslations(EntityTypeBuilder<WordEntity> builder)
    {
        builder.HasMany(entity => entity.Translations)
            .WithMany(parent => parent.Words)
            .UsingEntity(
                "word_translation",
                r => r.HasOne(typeof(TranslationEntity))
                    .WithMany()
                    .HasForeignKey("translation_id"),
                l => l.HasOne(typeof(WordEntity))
                    .WithMany()
                    .HasForeignKey("word_id"),
                j => j.ToTable(name: "word_translation")
            );
    }

    private void ConfigureColumns(EntityTypeBuilder<WordEntity> builder)
    {
        builder.Property(entity => entity.WordId)
            .HasColumnName("word_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(entity => entity.Word)
            .HasColumnName("word")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(entity => entity.WordTypeId)
            .HasColumnName("word_type_id")
            .IsRequired();
    }
}
