using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Exceptions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Common.Settings;
using R.Systems.Lexica.Core.Sets.Commands.CreateSet;
using R.Systems.Lexica.Persistence.Files.Common;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Persistence.Files.Sets.Commands.CreateSet;

internal class CreateSetRepository : ICreateSetRepository
{
    public CreateSetRepository(
        IOptionsSnapshot<LexicaSettings> lexicaSettings,
        SetSerializer setSerializer,
        ISetSource setSource)
    {
        LexicaSettings = lexicaSettings.Value;
        SetSerializer = setSerializer;
        SetSource = setSource;
    }

    public LexicaSettings LexicaSettings { get; }
    public SetSerializer SetSerializer { get; }
    public ISetSource SetSource { get; }

    public async Task CreateSetAsync(Set set)
    {
        var filePath = Path.Combine(LexicaSettings.SetFilesDirPath, set.Name);
        ValidateFileExistence(filePath);
        var serializedSet = SetSerializer.Serialize(set);
        await SetSource.CreateSetAsync(filePath, serializedSet);
    }

    private void ValidateFileExistence(string filePath)
    {
        if (SetSource.Exists(filePath))
        {
            ErrorInfo errorInfo = new(
                errorKey: "Exists",
                elementKey: "Set",
                data: new Dictionary<string, string>() { ["FilePath"] = filePath }
            );
            throw new ValidationException(errorInfo);
        }
    }
}
