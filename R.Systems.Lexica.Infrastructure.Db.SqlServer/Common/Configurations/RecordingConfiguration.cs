using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class RecordingConfiguration : IEntityTypeConfiguration<RecordingEntity>
{
    public void Configure(EntityTypeBuilder<RecordingEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureRelations(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<RecordingEntity> builder)
    {
        builder.ToTable(name: "recording");
    }

    private void SetPrimaryKey(EntityTypeBuilder<RecordingEntity> builder)
    {
        builder.HasKey(entity => entity.RecordingId);
    }

    private void ConfigureRelations(EntityTypeBuilder<RecordingEntity> builder)
    {
        ConfigureRelationWithWordType(builder);
    }

    private static void ConfigureRelationWithWordType(EntityTypeBuilder<RecordingEntity> builder)
    {
        builder.HasOne(entity => entity.WordType)
            .WithMany(parent => parent.Recordings)
            .HasForeignKey(entity => entity.WordTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureColumns(EntityTypeBuilder<RecordingEntity> builder)
    {
        builder.Property(entity => entity.RecordingId)
            .HasColumnName("recording_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(entity => entity.Word)
            .HasColumnName("word")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(entity => entity.WordTypeId)
            .HasColumnName("word_type_id")
            .IsRequired();

        builder.Property(entity => entity.FileName)
            .HasColumnName("file_name")
            .IsRequired()
            .HasMaxLength(200);
    }
}
