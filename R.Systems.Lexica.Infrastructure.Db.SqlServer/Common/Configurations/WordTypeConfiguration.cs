using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class WordTypeConfiguration : IEntityTypeConfiguration<WordTypeEntity>
{
    public void Configure(EntityTypeBuilder<WordTypeEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
        InitData(builder);
    }

    private void SetTableName(EntityTypeBuilder<WordTypeEntity> builder)
    {
        builder.ToTable(name: "word_type");
    }

    private void SetPrimaryKey(EntityTypeBuilder<WordTypeEntity> builder)
    {
        builder.HasKey(entity => entity.WordTypeId);
    }

    private void ConfigureColumns(EntityTypeBuilder<WordTypeEntity> builder)
    {
        builder.Property(entity => entity.WordTypeId)
            .HasColumnName("word_type_id")
            .IsRequired();

        builder.Property(entity => entity.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(entity => entity.Name)
            .IsUnique();
    }

    private void InitData(EntityTypeBuilder<WordTypeEntity> builder)
    {
        builder.HasData(
            new WordTypeEntity()
            {
                WordTypeId = 1,
                Name = WordTypes.None.ToString()
            },
            new WordTypeEntity()
            {
                WordTypeId = 2,
                Name = WordTypes.Noun.ToString()
            },
            new WordTypeEntity()
            {
                WordTypeId = 3,
                Name = WordTypes.Verb.ToString()
            },
            new WordTypeEntity()
            {
                WordTypeId = 4,
                Name = WordTypes.Adjective.ToString()
            },
            new WordTypeEntity()
            {
                WordTypeId = 5,
                Name = WordTypes.Adverb.ToString()
            }
        );
    }
}
