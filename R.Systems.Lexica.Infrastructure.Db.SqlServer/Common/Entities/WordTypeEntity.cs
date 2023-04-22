namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class WordTypeEntity
{
    public int WordTypeId { get; set; }

    public string Name { get; set; } = "";

    public ICollection<WordEntity> Words { get; set; } = new List<WordEntity>();
}
