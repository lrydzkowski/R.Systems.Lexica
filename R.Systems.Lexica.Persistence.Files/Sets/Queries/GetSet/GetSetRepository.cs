using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Exceptions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Common.Settings;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Persistence.Files.Sets.Common;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSet;

internal class GetSetRepository : IGetSetRepository
{
    public GetSetRepository(
        IOptionsSnapshot<LexicaSettings> lexicaSettings,
        SetContentValidator setContentValidator,
        ISetSource setSource)
    {
        LexicaSettings = lexicaSettings.Value;
        SetContentValidator = setContentValidator;
        SetSource = setSource;
    }

    public LexicaSettings LexicaSettings { get; }
    public SetContentValidator SetContentValidator { get; }
    public ISetSource SetSource { get; }

    public async Task<Set> GetSetAsync(string name)
    {
        string filePath = Path.Combine(LexicaSettings.SetFilesDirPath, name);
        ValidateSourceExistence(filePath);
        string fileContent = await SetSource.GetContentAsync(filePath);
        SetContentValidator.ValidateContent(fileContent, filePath);
        List<Entry> entries = ParseContent(fileContent);
        return new()
        {
            Name = name,
            Entries = entries
        };
    }

    private void ValidateSourceExistence(string filePath)
    {
        if (!SetSource.Exists(filePath))
        {
            ErrorInfo errorInfo = new(
                errorKey: "NotExist",
                elementKey: "SetFile",
                data: new Dictionary<string, string>() { ["FilePath"] = filePath }
            );
            throw new ValidationException(errorInfo);
        }
    }

    private List<Entry> ParseContent(string content)
    {
        List<Entry> entries = new();
        var lines = content.Split('\n');
        foreach (var line in lines)
        {
            var lineParts = line.Split(';');
            if (lineParts.Length != 2)
            {
                continue;
            }
            var words = lineParts[0].Split(',').Select(x => x.Trim()).ToList();
            var translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList();
            Entry entry = new()
            {
                Words = words,
                Translations = translations
            };
            entries.Add(entry);
        }
        return entries;
    }
}
