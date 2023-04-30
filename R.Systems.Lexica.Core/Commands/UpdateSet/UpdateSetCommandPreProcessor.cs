using MediatR.Pipeline;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.UpdateSet;

internal class UpdateSetCommandPreProcessor : IRequestPreProcessor<UpdateSetCommand>
{
    public Task Process(UpdateSetCommand request, CancellationToken cancellationToken)
    {
        request.SetName = request.SetName?.Trim() ?? "";
        foreach (Entry entry in request.Entries)
        {
            entry.Word = entry.Word?.Trim()?.ToLower() ?? "";
            entry.Translations = entry.Translations?.Select(translation => translation?.Trim()?.ToLower())
                                     .Where(translation => !string.IsNullOrWhiteSpace(translation))
                                     .Cast<string>()
                                     .Distinct()
                                     .ToList()
                                 ?? new List<string>();
        }

        return Task.CompletedTask;
    }
}
