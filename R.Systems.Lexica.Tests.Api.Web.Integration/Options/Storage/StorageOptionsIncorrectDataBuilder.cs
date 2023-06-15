using R.Systems.Lexica.Infrastructure.Storage.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Storage;

internal class StorageOptionsIncorrectDataBuilder
    : IncorrectDataBuilderBase<StorageOptionsData, StorageOptionsTestsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new StorageOptionsData
                {
                    DirectoryPath = ""
                },
                new StorageOptionsTestsData
                {
                    DirectoryExists = true
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(StorageOptions.Position, nameof(StorageOptions.DirectoryPath))
                    }
                )
            ),
            BuildParameters(
                2,
                new StorageOptionsData
                {
                    DirectoryPath = " "
                },
                new StorageOptionsTestsData
                {
                    DirectoryExists = true
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(StorageOptions.Position, nameof(StorageOptions.DirectoryPath))
                    }
                )
            ),
            BuildParameters(
                3,
                new StorageOptionsData
                {
                    DirectoryPath = "/recordings"
                },
                new StorageOptionsTestsData
                {
                    DirectoryExists = false
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildErrorMessage(
                            $"{StorageOptions.Position}.{nameof(StorageOptions.DirectoryPath)}",
                            $"'{nameof(StorageOptions.DirectoryPath)}' has to be a path to an existing directory."
                        )
                    }
                )
            )
        };
    }
}
