using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Queries.GetRecording;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Api.Web.Mappers;

[Mapper]
public partial class GetRecordingMapper
{
    public partial GetRecordingQuery MapToCommand(GetRecordingRequest request);

    private WordType MapToWordType(string wordType) => WordTypeMapper.MapToWordType(wordType);
}
