using System.Linq.Dynamic.Core;

namespace R.Systems.Lexica.Core.Common.Lists.Extensions;

public static class SortExtensions
{
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> query,
        List<string> fieldsAvailableToSort,
        Sorting sorting,
        string defaultSortingFieldName
    )
    {
        sorting = PrepareSortingParameters(sorting, defaultSortingFieldName);

        string fieldName = sorting.FieldName!.FirstCharToUpper();
        if (!fieldsAvailableToSort.Contains(fieldName))
        {
            return query;
        }

        string sortOrderQuery = sorting.Order == SortingOrder.Ascending ? "" : " desc";
        string sortQuery = $"{fieldName}{sortOrderQuery}";
        query = query.OrderBy(sortQuery);

        return query;
    }

    private static Sorting PrepareSortingParameters(Sorting sorting, string defaultSortingFieldName)
    {
        if (sorting.FieldName != null)
        {
            return sorting;
        }

        return new Sorting
        {
            FieldName = defaultSortingFieldName,
            Order = SortingOrder.Ascending
        };
    }

    private static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
}
