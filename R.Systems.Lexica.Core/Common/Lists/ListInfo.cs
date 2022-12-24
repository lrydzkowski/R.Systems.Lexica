namespace R.Systems.Lexica.Core.Common.Lists;

public class ListInfo<T> where T : new()
{
    public int Count { get; init; }

    public List<T> Data { get; init; } = new();
}
