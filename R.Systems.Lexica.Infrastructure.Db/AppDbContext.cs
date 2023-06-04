using Microsoft.EntityFrameworkCore;
using System.Reflection;
using R.Systems.Lexica.Infrastructure.Db.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db;

internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<WordTypeEntity> WordTypes => Set<WordTypeEntity>();

    public DbSet<WordEntity> WordEntities => Set<WordEntity>();

    public DbSet<TranslationEntity> TranslationEntities => Set<TranslationEntity>();

    public DbSet<SetEntity> SetEntities => Set<SetEntity>();

    public DbSet<RecordingEntity> RecordingEntities => Set<RecordingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
