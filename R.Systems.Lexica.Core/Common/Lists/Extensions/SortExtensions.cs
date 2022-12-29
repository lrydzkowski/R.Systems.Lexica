using R.Systems.Lexica.Core.Common.Extensions;
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
        string fieldToSort = sorting.FieldName!.FirstCharToUpper();

        if (!fieldsAvailableToSort.Contains(fieldToSort))
        {
            return query;
        }

        string sortOrderQuery = sorting.Order == SortingOrder.Ascending ? "" : " desc";
        string sortQuery = $"{fieldToSort}{sortOrderQuery}, {defaultSortingFieldName} asc";
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
}
