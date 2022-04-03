using AutoMapper;
using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Commands.CreateSet;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateSetEntryCommand, Entry>();
        CreateMap<CreateSetCommand, Set>();
    }
}
