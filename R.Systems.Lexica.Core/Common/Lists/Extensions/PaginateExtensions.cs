namespace R.Systems.Lexica.Core.Common.Lists.Extensions;

public static class PaginateExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination)
    {
        if (pagination.PageSize > -1)
        {
            query = query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
        }

        return query;
    }
}
