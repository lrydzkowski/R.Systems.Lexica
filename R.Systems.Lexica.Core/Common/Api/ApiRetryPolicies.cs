using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Retry;
using Polly.Wrap;
using RestSharp;

namespace R.Systems.Lexica.Core.Common.Api;

public interface IApiRetryPolicies<TLogger>
{
    Task<TResult> ExecuteWithRetryPolicyAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Func<TResult, bool> retryPredicate,
        int retryCount,
        CancellationToken cancellationToken = default
    ) where TResult : RestResponse;

    Task<TResult> ExecuteWithRetryPolicyAndCacheAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Func<TResult, bool> retryPredicate,
        int retryCount,
        string cacheKey,
        CancellationToken cancellationToken = default
    ) where TResult : RestResponse;
}

internal class ApiRetryPolicies<TLogger>
    : IApiRetryPolicies<TLogger>
{
    private readonly ILogger<TLogger> _logger;
    private readonly IAsyncCacheProvider _asyncCacheProvider;

    public ApiRetryPolicies(ILogger<TLogger> logger, IAsyncCacheProvider asyncCacheProvider)
    {
        _logger = logger;
        _asyncCacheProvider = asyncCacheProvider;
    }

    public async Task<TResult> ExecuteWithRetryPolicyAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Func<TResult, bool> retryPredicate,
        int retryCount,
        CancellationToken cancellationToken = default
    ) where TResult : RestResponse
    {
        AsyncRetryPolicy<TResult> retryPolicy = DefineRetryPolicy(retryPredicate, retryCount);

        return await retryPolicy.ExecuteAsync(action, new Context(), cancellationToken);
    }

    public async Task<TResult> ExecuteWithRetryPolicyAndCacheAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Func<TResult, bool> retryPredicate,
        int retryCount,
        string cacheKey,
        CancellationToken cancellationToken = default
    ) where TResult : RestResponse
    {
        AsyncPolicyWrap<TResult> policyWrap =
            Policy.WrapAsync(DefineCachePolicy<TResult>(), DefineRetryPolicy(retryPredicate, retryCount));

        return await policyWrap.ExecuteAsync(
            action,
            new Context(cacheKey),
            cancellationToken
        );
    }

    private AsyncCachePolicy<TResult> DefineCachePolicy<TResult>()
    {
        return Policy.CacheAsync<TResult>(_asyncCacheProvider, TimeSpan.FromHours(24));
    }

    private AsyncRetryPolicy<TResult> DefineRetryPolicy<TResult>(Func<TResult, bool> resultPredicate, int retryCount)
        where TResult : RestResponse
    {
        return Policy
            .HandleResult(resultPredicate)
            .WaitAndRetryAsync(
                retryCount,
                _ => TimeSpan.FromSeconds(3),
                (response, timeSpan, definedRetryCount, _) =>
                {
                    _logger.LogWarning(
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
