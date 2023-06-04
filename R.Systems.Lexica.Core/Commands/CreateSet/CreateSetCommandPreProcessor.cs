using MediatR.Pipeline;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.CreateSet;

internal class CreateSetCommandPreProcessor : IRequestPreProcessor<CreateSetCommand>
{
    public Task Process(CreateSetCommand request, CancellationToken cancellationToken)
    {
        request.SetName = request.SetName?.Trim() ?? "";
        foreach (Entry entry in request.Entries)
        {
            entry.Word = entry.Word?.Trim()?.ToLower() ?? "";
            entry.Translations = entry.Translations?.Select(translation => translation?.Trim()?.ToLower())
                                     .Where(translation => !string.IsNullOrWhiteSpace(translation))
                                     .Distinct()
                                     .Cast<string>()
                                     .ToList()
                                 ?? new List<string>();
        }

        return Task.CompletedTask;
    }
}
