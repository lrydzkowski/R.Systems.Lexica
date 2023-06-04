namespace R.Systems.Lexica.Infrastructure.Db.Common.Entities;

internal class SetEntity
{
    public long SetId { get; set; }

    public string Name { get; set; } = "";

    public DateTimeOffset CreatedAtUtc { get; set; }

    public ICollection<WordEntity> Words { get; set; } = new List<WordEntity>();
}
