namespace R.Systems.Lexica.Core.Queries.GetSets;

public class SetRecordDto
{
    public long SetId { get; init; }

    public string Name { get; init; } = "";

    public DateTimeOffset CreatedAt { get; init; }
}
