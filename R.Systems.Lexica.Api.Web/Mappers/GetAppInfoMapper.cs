using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Queries.GetAppInfo;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Api.Web.Mappers;

[Mapper]
public partial class GetAppInfoMapper
{
    public partial GetAppInfoResponse ToResponse(GetAppInfoResult result);
}
