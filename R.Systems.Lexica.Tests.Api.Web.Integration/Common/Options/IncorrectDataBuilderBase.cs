namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

internal abstract class IncorrectDataBuilderBase<TOptionsData> where TOptionsData : IOptionsData
{
    protected static string BuildExpectedExceptionMessage(List<string> exceptionMessageParts)
    {
        exceptionMessageParts = exceptionMessageParts.Select(x => $" -- {x}").ToList();
        exceptionMessageParts.Insert(0, "App settings - Validation failed: ");

        return string.Join(Environment.NewLine, exceptionMessageParts);
    }

    protected static object[] BuildParameters(int id, TOptionsData optionsData, string expectedExceptionMessage)
    {
        return new object[] { id, optionsData.ConvertToInMemoryCollection(), expectedExceptionMessage };
    }

    protected static string BuildNotEmptyErrorMessage(string position, string propertyName)
    {
        return BuildNotEmptyErrorMessage(new[] { position }, propertyName);
    }

    protected static string BuildNotEmptyErrorMessage(string[] positions, string propertyName)
    {
        return $"{string.Join('.', positions)}.{propertyName}: '{propertyName}' must not be empty. Severity: Error";
    }

    protected static string BuildErrorMessage(string property, string errorMsg)
    {
        return $"{property}: {errorMsg} Severity: Error";
    }
}

internal abstract class IncorrectDataBuilderBase<TOptionsData, TData> : IncorrectDataBuilderBase<TOptionsData>
    where TOptionsData : IOptionsData
    where TData : class
{
    protected static object[] BuildParameters(
        int id,
        TOptionsData optionsData,
        TData testData,
        string expectedExceptionMessage
    )
    {
        return new object[] { id, optionsData.ConvertToInMemoryCollection(), testData, expectedExceptionMessage };
    }
}
