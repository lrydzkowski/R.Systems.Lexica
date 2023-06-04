using RestSharp;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Api;

internal class ApiDataBuilder
{
    public static IEnumerable<object?[]> Build()
    {
        return new List<object?[]>
        {
            new object?[] { "/recordings/test?wordType=noun", Method.Get },
            new object?[] { "/sets", Method.Get },
            new object?[] { "/sets/1", Method.Get },
            new object?[] { "/sets/1", Method.Delete },
            new object?[] { "/sets", Method.Post },
            new object?[] { "/sets", Method.Put },
            new object?[] { "/words/test/definitions", Method.Get }
        };
    }
}
