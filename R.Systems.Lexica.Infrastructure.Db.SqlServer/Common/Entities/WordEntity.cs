namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class WordEntity
{
    public long WordId { get; set; }

    public string Word { get; set; } = "";

    public int WordTypeId { get; set; }
    public WordTypeEntity? WordType { get; set; }

    public int Order { get; set; }

    public long SetId { get; set; }
    public SetEntity? Set { get; set; }

    public ICollection<TranslationEntity> Translations { get; set; } = new List<TranslationEntity>();
}
