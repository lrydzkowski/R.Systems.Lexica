namespace R.Systems.Lexica.Tests.Integration.Common.Options;

internal interface IOptionsData
{
    Dictionary<string, string?> ConvertToInMemoryCollection();
}
