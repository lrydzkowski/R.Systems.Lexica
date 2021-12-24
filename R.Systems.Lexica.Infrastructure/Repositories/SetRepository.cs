using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Interfaces;
using R.Systems.Lexica.Core.Models;
using R.Systems.Lexica.Infrastructure.Settings;
using R.Systems.Lexica.Infrastructure.Validators;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Infrastructure.Repositories;

public class SetRepository : ISetRepository
{
    public SetRepository(
        IOptionsSnapshot<InfrastructureSettings> infrastructureSettings,
        SetValidator setValidator)
    {
        InfrastructureSettings = infrastructureSettings.Value;
        SetValidator = setValidator;
    }

    public InfrastructureSettings InfrastructureSettings { get; }
    public SetValidator SetValidator { get; }

    public async Task<OperationResult<Set?>> GetSetAsync(string name)
    {
        string filePath = Path.Combine(InfrastructureSettings.SetFilesDirPath, name);
        if (!SetValidator.SetFileExists(filePath))
        {
            return new OperationResult<Set?> { Result = false };
        }
        string? fileContent = await File.ReadAllTextAsync(filePath);
        if (!SetValidator.ValidateSetFileContent(fileContent, filePath))
        {
            return new OperationResult<Set?> { Result = false };
        }
        List<Entry> entries = new();
        string[] lines = fileContent.Split('\n');
        foreach (string line in lines)
        {
            string[] lineParts = line.Split(';');
            if (lineParts.Length != 2)
            {
                continue;
            }
            List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList();
            List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList();
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

    public async Task<OperationResult<List<Set>>> GetSetsAsync()
    {
        bool result = true;
        List<Set> sets = new();
        DirectoryInfo dirInfo = new(InfrastructureSettings.SetFilesDirPath);
        FileInfo[] files = dirInfo.GetFiles("*.*");
        foreach (FileInfo file in files)
        {
            OperationResult<Set?> gettingSetResult = await GetSetAsync(file.Name);
            result &= gettingSetResult.Result;
            if (gettingSetResult.Data == null)
            {
                continue;
            }
            sets.Add(gettingSetResult.Data);
        }
        return new OperationResult<List<Set>> { Result = result, Data = sets };
    }
}
