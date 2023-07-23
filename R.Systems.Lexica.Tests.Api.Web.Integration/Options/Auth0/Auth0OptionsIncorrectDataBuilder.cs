using R.Systems.Lexica.Infrastructure.Auth0.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Auth0;

internal class Auth0OptionsIncorrectDataBuilder : IncorrectDataBuilderBase<Auth0OptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new Auth0OptionsData
                {
                    Domain = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.Domain))
                    }
                )
            ),
            BuildParameters(
                2,
                new Auth0OptionsData
                {
                    Domain = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.Domain))
                    }
                )
            ),
            BuildParameters(
                3,
                new Auth0OptionsData
                {
                    Audience = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.Audience))
                    }
                )
            ),
            BuildParameters(
                4,
                new Auth0OptionsData
                {
                    Audience = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.Audience))
                    }
                )
            ),
            BuildParameters(
                5,
                new Auth0OptionsData
                {
                    RoleClaimName = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.RoleClaimName))
                    }
                )
            ),
            BuildParameters(
                6,
                new Auth0OptionsData
                {
                    RoleClaimName = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(Auth0Options.Position, nameof(Auth0Options.RoleClaimName))
                    }
                )
            )
        };
    }
}
