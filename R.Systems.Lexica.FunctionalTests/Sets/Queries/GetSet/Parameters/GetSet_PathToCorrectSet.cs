using System.Collections.Generic;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.FunctionalTests.Common.Settings;

namespace R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSet.Parameters;

internal static class GetSet_PathToCorrectSet
{
    public static IEnumerable<object[]> Get()
    {
        return new List<object[]>
        {
            new object[]
            {
                AssetsPaths.SetCorrectFilesDirPath,
                "example_set_1.txt",
                31,
                new Entry
                {
                    Words = new List<string>() { "incomparably" },
                    Translations = new List<string>() { "nieporównywalnie" }
                },
                new Entry
                {
                    Words = new List<string>() { "mist" },
                    Translations = new List<string>() { "mgła" }
                }
            },
            new object[]
            {
                AssetsPaths.SetCorrectFilesDirPath,
                "example_set_5.txt",
                28,
                new Entry
                {
                    Words = new List<string>() { "jagged" },
                    Translations = new List<string>() { "postrzępiony", "wyszczerbiony" }
                },
                new Entry
                {
                    Words = new List<string>() { "misfit", "creep" },
                    Translations = new List<string>() { "odmieniec" }
                }
            }
        };
    }
}
