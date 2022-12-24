﻿namespace R.Systems.Lexica.Core.Common.Lists;

public class ListParameters
{
    public Pagination Pagination { get; init; } = new();

    public Sorting Sorting { get; init; } = new();

    public Search Search { get; init; } = new();
}

public class Pagination
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 100;
}

public class Sorting
{
    public string? FieldName { get; init; }

    public SortingOrder Order { get; init; } = SortingOrder.Ascending;
}

public enum SortingOrder
{
    Ascending,
    Descending
}

public class Search
{
    public string? Query { get; init; }
}
