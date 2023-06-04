namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

internal class OptionsPositionsMerger
{
    public static string Merge(string? parentPosition, string position, string separator = ":")
    {
        if (string.IsNullOrWhiteSpace(parentPosition))
        {
            return position.Trim();
        }

        string mergedPosition = string.Join(separator, parentPosition, position);

        return mergedPosition;
    }
}
