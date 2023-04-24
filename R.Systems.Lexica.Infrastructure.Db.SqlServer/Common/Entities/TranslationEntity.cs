namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class TranslationEntity
{
    public long TranslationId { get; set; }

    public string Translation { get; set; } = "";

    public ICollection<WordEntity> Words { get; set; } = new List<WordEntity>();
}
