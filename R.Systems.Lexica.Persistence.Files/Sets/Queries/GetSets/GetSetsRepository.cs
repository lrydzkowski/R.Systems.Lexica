using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Exceptions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Common.Settings;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Persistence.Files.Common;
using R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSet;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSets;

internal class GetSetsRepository : IGetSetsRepository
{
    public GetSetsRepository(
        IOptionsSnapshot<LexicaSettings> lexicaSettings,
        SetContentValidator setValidator,
        ISetSource setSource,
        IGetSetRepository getSetRepository)
    {
        LexicaSettings = lexicaSettings.Value;
        SetValidator = setValidator;
        SetSource = setSource;
        GetSetRepository = getSetRepository;
    }

    public LexicaSettings LexicaSettings { get; }
    public SetContentValidator SetValidator { get; }
    public ISetSource SetSource { get; }
    public IGetSetRepository GetSetRepository { get; }

    public async Task<List<Set>> GetSetsAsync()
    {
        string setFilesDirPath = LexicaSettings.SetFilesDirPath;
        ValidateDirExistence(setFilesDirPath);
        List<Set> sets = new();
        List<ErrorInfo> errors = new();
        var setNames = SetSource.GetSetNames(setFilesDirPath);
        foreach (var setName in setNames)
        {
            try
            {
                var set = await GetSetRepository.GetSetAsync(setName);
                sets.Add(set);
            }
            catch (ValidationException ex)
            {
                errors.AddRange(ex.Errors);
            }
        }
        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
        return sets;
    }

    private void ValidateDirExistence(string setFilesDirPath)
    {
        if (!SetSource.DirExists(setFilesDirPath))
        {
            ErrorInfo errorInfo = new(
                errorKey: "NotExist",
                elementKey: "SetDir",
                data: new Dictionary<string, string>() { ["DirPath"] = setFilesDirPath }
            );
            throw new ValidationException(errorInfo);
        }
    }
}
