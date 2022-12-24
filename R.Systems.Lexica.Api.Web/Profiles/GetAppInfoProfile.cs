using AutoMapper;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;

namespace R.Systems.Lexica.Api.Web.Profiles;

public class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
