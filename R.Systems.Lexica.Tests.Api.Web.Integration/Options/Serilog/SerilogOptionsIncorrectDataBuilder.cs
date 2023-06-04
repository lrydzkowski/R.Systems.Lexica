using R.Systems.Lexica.Api.Web.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Serilog;

internal class SerilogOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<SerilogOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new SerilogOptionsData
                {
                    StorageAccountData = new SerilogStorageAccountOptionsData
                    {
                        ConnectionString = " "
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { SerilogOptions.Position, SerilogStorageAccountOptions.Position },
                            nameof(SerilogStorageAccountOptions.ConnectionString)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new SerilogOptionsData
                {
                    StorageAccountData = new SerilogStorageAccountOptionsData
                    {
                        ContainerName = ""
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { SerilogOptions.Position, SerilogStorageAccountOptions.Position },
                            nameof(SerilogStorageAccountOptions.ContainerName)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new SerilogOptionsData
                {
                    StorageAccountData = new SerilogStorageAccountOptionsData
                    {
                        ContainerName = " "
                    }
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            new[] { SerilogOptions.Position, SerilogStorageAccountOptions.Position },
                            nameof(SerilogStorageAccountOptions.ContainerName)
                        )
                    }
                )
            )
        };
    }
}
