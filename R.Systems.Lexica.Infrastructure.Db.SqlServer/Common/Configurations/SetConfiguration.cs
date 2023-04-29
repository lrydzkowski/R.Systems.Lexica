using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class SetConfiguration : IEntityTypeConfiguration<SetEntity>
{
    public void Configure(EntityTypeBuilder<SetEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<SetEntity> builder)
    {
        builder.ToTable(name: "set");
    }

    private void SetPrimaryKey(EntityTypeBuilder<SetEntity> builder)
    {
        builder.HasKey(entity => entity.SetId);
    }

    private void ConfigureColumns(EntityTypeBuilder<SetEntity> builder)
    {
        builder.Property(entity => entity.SetId)
            .HasColumnName("set_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(entity => entity.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(entity => entity.Name)
            .IsUnique();

        builder.Property(entity => entity.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();
    }
}
