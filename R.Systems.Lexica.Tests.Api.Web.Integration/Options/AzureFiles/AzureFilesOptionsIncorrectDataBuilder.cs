using R.Systems.Lexica.Infrastructure.Azure.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureFiles;

internal class AzureFilesOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<AzureFilesOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new AzureFilesOptionsData
                {
                    ConnectionString = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            AzureFilesOptions.Position,
                            nameof(AzureFilesOptions.ConnectionString)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new AzureFilesOptionsData
                {
                    ConnectionString = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            AzureFilesOptions.Position,
                            nameof(AzureFilesOptions.ConnectionString)
                        )
                    }
                )
            )
        };
    }
}
