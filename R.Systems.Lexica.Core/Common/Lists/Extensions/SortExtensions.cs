﻿using System.Linq.Dynamic.Core;

namespace R.Systems.Lexica.Core.Common.Lists.Extensions;

public static class SortExtensions
{
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        List<string> fieldsAvailableToSort,
        Sorting sorting,
        string defaultSortingFieldName,
        Dictionary<string, string>? fieldNamesMapping = null
    )
    {
        if (!fieldsAvailableToSort.Contains(sorting.FieldName!))
        {
            return query;
        }

        sorting = PrepareSortingParameters(sorting, defaultSortingFieldName, fieldNamesMapping);
        string sortOrderQuery = sorting.Order == SortingOrder.Ascending ? "" : " desc";
        string sortQuery = $"{sorting.FieldName}{sortOrderQuery}, {defaultSortingFieldName} asc";
        query = query.OrderBy(sortQuery);

        return query;
    }

    private static Sorting PrepareSortingParameters(
        Sorting sorting,
        string defaultSortingFieldName,
        Dictionary<string, string>? fieldNamesMapping = null
    )
    {
        if (sorting.FieldName == null)
        {
            return new Sorting
            {
                FieldName = defaultSortingFieldName,
                Order = SortingOrder.Ascending
            };
        }

        sorting = MapFieldName(sorting, fieldNamesMapping);

        return sorting;
    }

    private static Sorting MapFieldName(Sorting sorting, Dictionary<string, string>? fieldNamesMapping = null)
    {
        string fieldName = sorting.FieldName ?? "";
        if (fieldNamesMapping?.TryGetValue(fieldName, out string? value) is true)
        {
            sorting.FieldName = value;
        }

        return sorting;
    }
}
