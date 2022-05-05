using System.IO;

namespace R.Systems.Lexica.FunctionalTests.Common.Settings;

internal static class AssetsPaths
{
    public static string SetFilesDirPath { get; } = "Assets/Sets";

    public static string SetCorrectFilesDirPath
    {
        get
        {
            return Path.Combine(SetFilesDirPath, "Correct");
        }
    }
}
