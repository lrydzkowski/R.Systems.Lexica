using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Interfaces;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Common.Settings;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Infrastructure.Persistence.Files.Sets;

public class SetRepository : ISetRepository
{
    public SetRepository(
        IOptionsSnapshot<LexicaSettings> lexicaSettings,
        SetValidator setValidator,
        ISetSource setSource)
    {
        LexicaSettings = lexicaSettings.Value;
        SetValidator = setValidator;
        SetSource = setSource;
    }

    public LexicaSettings LexicaSettings { get; }
    public SetValidator SetValidator { get; }
    public ISetSource SetSource { get; }

    public async Task<OperationResult<List<Set>?>> GetSetsAsync()
    {
        string setFilesDirPath = LexicaSettings.SetFilesDirPath;
        if (!SetValidator.SetDirExists(setFilesDirPath))
        {
            return new OperationResult<List<Set>?> { Result = false };
        }
        var result = true;
        List<Set> sets = new();
        var setNames = SetSource.GetSetNames(setFilesDirPath);
        foreach (var setName in setNames)
        {
            var gettingSetResult = await GetSetAsync(setName);
            result &= gettingSetResult.Result;
            if (gettingSetResult.Data == null)
            {
                continue;
            }
            sets.Add(gettingSetResult.Data);
        }
        return new OperationResult<List<Set>?> { Result = result, Data = sets };
    }

    public async Task<OperationResult<Set?>> GetSetAsync(string name)
    {
        var filePath = Path.Combine(LexicaSettings.SetFilesDirPath, name);
        if (!SetValidator.SetFileExists(filePath))
        {
            return new OperationResult<Set?> { Result = false };
        }
        var fileContent = await SetSource.GetContentAsync(filePath);
        if (!SetValidator.ValidateSetFileContent(fileContent, filePath))
        {
            return new OperationResult<Set?> { Result = false };
        }
        List<Entry> entries = new();
        var lines = fileContent.Split('\n');
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
        Set set = new()
        {
            Name = name,
            Entries = entries
        };
        return new OperationResult<Set?>() { Result = true, Data = set };
    }
}
