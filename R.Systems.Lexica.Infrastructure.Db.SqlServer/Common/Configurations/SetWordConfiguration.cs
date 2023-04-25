using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Configurations;

internal class SetWordConfiguration : IEntityTypeConfiguration<SetWordEntity>
{
    public void Configure(EntityTypeBuilder<SetWordEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<SetWordEntity> builder)
    {
        builder.ToTable(name: "set_word");
    }

    private void SetPrimaryKey(EntityTypeBuilder<SetWordEntity> builder)
    {
        builder.HasKey(entity => new { entity.SetId, entity.WordId });
    }

    private void ConfigureColumns(EntityTypeBuilder<SetWordEntity> builder)
    {
        builder.Property(entity => entity.SetId)
            .HasColumnName("set_id")
            .IsRequired();

        builder.Property(entity => entity.WordId)
            .HasColumnName("word_id")
            .IsRequired();

        builder.Property(entity => entity.Order)
            .HasColumnName("order")
            .IsRequired();
    }
}
