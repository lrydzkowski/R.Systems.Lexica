namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

internal interface IOptionsData
{
    Dictionary<string, string?> ConvertToInMemoryCollection();
}
