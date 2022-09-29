using AutoMapper;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;
using R.Systems.Lexica.WebApi.Api;

namespace R.Systems.Lexica.WebApi.Profiles;

public class GetAppInfoProfile : Profile
{
    public GetAppInfoProfile()
    {
        CreateMap<GetAppInfoResult, GetAppInfoResponse>();
    }
}
