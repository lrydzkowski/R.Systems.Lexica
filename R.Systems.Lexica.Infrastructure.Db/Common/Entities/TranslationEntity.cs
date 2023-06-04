namespace R.Systems.Lexica.Infrastructure.Db.Common.Entities;

internal class TranslationEntity
{
    public long TranslationId { get; set; }

    public string Translation { get; set; } = "";

    public int Order { get; set; }

    public long WordId { get; set; }
    public WordEntity? Word { get; set; }
}
