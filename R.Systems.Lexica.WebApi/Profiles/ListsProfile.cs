using AutoMapper;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.WebApi.Api;

namespace R.Systems.Lexica.WebApi.Profiles;

public class ListsProfile : Profile
{
    public ListsProfile()
    {
        CreateMap<ListRequest, ListParameters>()
            .ForMember(
                parameters => parameters.Pagination,
                options => options.MapFrom(
                    request => new Pagination { Page = request.Page, PageSize = request.PageSize }
                )
            )
            .ForMember(
                parameters => parameters.Search,
                options => options.MapFrom(request => new Search { Query = request.SearchQuery })
            )
            .ForMember(
                parameters => parameters.Sorting,
                options => options.MapFrom(
                    request => new Sorting
                    { FieldName = request.SortingFieldName, Order = MapToSortingOrder(request.SortingOrder) }
                )
            );
    }

    private SortingOrder MapToSortingOrder(string sortingOrder)
    {
        switch (sortingOrder)
        {
            case "desc":
                return SortingOrder.Descending;
            default:
                return SortingOrder.Ascending;
        }
    }
}
