using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RestSharp;

namespace R.Systems.Lexica.Core.Common.Api;

public interface IApiRetryPolicies
{
    Task<RestResponse<T?>> ExecuteWithRetryPolicyAsync<T>(
        RestClient client,
        RestRequest restRequest,
        ILogger logger,
        int retryCount,
        CancellationToken cancellationToken = default
    );

    Task<RestResponse> ExecuteWithRetryPolicyAsync(
        RestClient client,
        RestRequest restRequest,
        ILogger logger,
        int retryCount,
        CancellationToken cancellationToken = default
    );
}

internal class ApiRetryPolicies
    : IApiRetryPolicies
{
    public async Task<RestResponse<T?>> ExecuteWithRetryPolicyAsync<T>(
        RestClient client,
        RestRequest restRequest,
        ILogger logger,
        int retryCount,
        CancellationToken cancellationToken = default
    )
    {
        AsyncRetryPolicy<RestResponse<T?>> retryPolicy = DefineRetryPolicy<T>(retryCount, logger);

        return await retryPolicy.ExecuteAsync(
            async (_, c) => await client.ExecuteAsync<T?>(restRequest, c),
            new Context(),
            cancellationToken
        );
    }

    public async Task<RestResponse> ExecuteWithRetryPolicyAsync(
        RestClient client,
        RestRequest restRequest,
        ILogger logger,
        int retryCount,
        CancellationToken cancellationToken = default
    )
    {
        AsyncRetryPolicy<RestResponse> retryPolicy = DefineRetryPolicy(retryCount, logger);

        return await retryPolicy.ExecuteAsync(
            async (_, c) => await client.ExecuteAsync(restRequest, c),
            new Context(),
            cancellationToken
        );
    }

    private AsyncRetryPolicy<RestResponse<T?>> DefineRetryPolicy<T>(int retryCount, ILogger logger)
    {
        return Policy
            .HandleResult<RestResponse<T?>>(x => !x.IsSuccessful)
            .WaitAndRetryAsync(
                retryCount,
                _ => TimeSpan.FromSeconds(3),
                (response, timeSpan, definedRetryCount, _) =>
                {
                    logger.LogWarning(
                        "API request failed. HttpStatusCode = {StatusCode}. Waiting {TimeSpan} seconds before retry. Number attempt {RetryCount}. Uri = {Uri}. RequestResponse = {RequestResponse}.",
                        response.Result.StatusCode,
                        timeSpan,
                        definedRetryCount,
                        response.Result.ResponseUri,
                        response.Result.Content
                    );
                }
            );
    }

    private AsyncRetryPolicy<RestResponse> DefineRetryPolicy(int retryCount, ILogger logger)
    {
        return Policy
            .HandleResult<RestResponse>(x => !x.IsSuccessful)
            .WaitAndRetryAsync(
                retryCount,
                _ => TimeSpan.FromSeconds(3),
                (response, timeSpan, definedRetryCount, _) =>
                {
                    logger.LogWarning(
                        "API request failed. HttpStatusCode = {StatusCode}. Waiting {TimeSpan} seconds before retry. Number attempt {RetryCount}. Uri = {Uri}. RequestResponse = {RequestResponse}.",
                        response.Result.StatusCode,
                        timeSpan,
                        definedRetryCount,
                        response.Result.ResponseUri,
                        response.Result.Content
                    );
                }
            );
    }
}
