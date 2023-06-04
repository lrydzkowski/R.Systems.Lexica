using R.Systems.Lexica.Infrastructure.Azure.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureStorage;

internal class AzureStorageOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<AzureStorageOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new AzureStorageOptionsData
                {
                    BlobData = new AzureStorageBlobOptionsData
                    {
                        ConnectionString = ""
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { AzureStorageOptions.Position, AzureStorageBlobOptions.Position },
                            nameof(AzureStorageBlobOptions.ConnectionString)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new AzureStorageOptionsData
                {
                    BlobData = new AzureStorageBlobOptionsData
                    {
                        ConnectionString = " "
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { AzureStorageOptions.Position, AzureStorageBlobOptions.Position },
                            nameof(AzureStorageBlobOptions.ConnectionString)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new AzureStorageOptionsData
                {
                    BlobData = new AzureStorageBlobOptionsData
                    {
                        ContainerName = " "
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { AzureStorageOptions.Position, AzureStorageBlobOptions.Position },
                            nameof(AzureStorageBlobOptions.ContainerName)
                        )
                    }
                )
            ),
            BuildParameters(
                4,
                new AzureStorageOptionsData
                {
                    BlobData = new AzureStorageBlobOptionsData
                    {
                        ContainerName = " "
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { AzureStorageOptions.Position, AzureStorageBlobOptions.Position },
                            nameof(AzureStorageBlobOptions.ContainerName)
                        )
                    }
                )
            )
        };
    }
}
