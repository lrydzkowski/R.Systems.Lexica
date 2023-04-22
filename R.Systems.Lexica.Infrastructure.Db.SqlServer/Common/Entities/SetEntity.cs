namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class SetEntity
{
    public long SetId { get; set; }

    public string Name { get; set; } = "";

    public DateTimeOffset CreatedAtUtc { get; set; }

    public ICollection<WordEntity> Words { get; set; } = new List<WordEntity>();

    public ICollection<TranslationEntity> Translations { get; set; } = new List<TranslationEntity>();
}
