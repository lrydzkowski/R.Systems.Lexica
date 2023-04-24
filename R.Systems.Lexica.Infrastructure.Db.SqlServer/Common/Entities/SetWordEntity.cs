namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

internal class SetWordEntity
{
    public long SetId { get; set; }
    public SetEntity? Set { get; set; }

    public long WordId { get; set; }
    public WordEntity? Word { get; set; }

    public int Order { get; set; }
}
