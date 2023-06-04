namespace R.Systems.Lexica.Infrastructure.Db.Common.Entities;

internal class RecordingEntity
{
    public long RecordingId { get; set; }

    public string Word { get; set; } = "";

    public int WordTypeId { get; set; }
    public WordTypeEntity? WordType { get; set; }

    public string FileName { get; set; } = "";
}
