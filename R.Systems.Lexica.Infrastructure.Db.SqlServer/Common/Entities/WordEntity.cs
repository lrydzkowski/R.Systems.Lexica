namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class WordEntity
{
    public long WordId { get; set; }

    public string Word { get; set; } = "";

    public int WordTypeId { get; set; }
    public WordTypeEntity? WordType { get; set; }

    public ICollection<SetWordEntity> SetWords { get; set; } = new List<SetWordEntity>();
    public ICollection<SetEntity> Sets { get; set; } = new List<SetEntity>();

    public ICollection<TranslationEntity> Translations { get; set; } = new List<TranslationEntity>();
}
