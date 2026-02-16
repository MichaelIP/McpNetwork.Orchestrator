using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// RetriableFeature is an abstract base class for features that require retry logic in case of transient failures.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class RetriableFeature<TInput, TResult> : IRetriableFeature<TInput, TResult> 
{

    /// <summary>
    /// RetryPolicy defines the retry behavior for the feature, including the maximum number of 
    /// retries and the delay between retries.
    /// </summary>
    public virtual RetryPolicy RetryPolicy { get; }

    /// <summary>
    /// Initializes a new instance of the RetriableFeature class with the specified retry policy.
    /// </summary>
    /// <param name="retryPolicy"></param>
    protected RetriableFeature(RetryPolicy? retryPolicy = null)
    {
        RetryPolicy = retryPolicy ?? new RetryPolicy();
    }

    /// <summary>
    /// Executes the feature with the given input and orchestration context, applying the retry logic defined in the RetryPolicy.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<FeatureResult<TResult>> ExecuteAsync(TInput input, OrchestrationContext context)
    {
        int attempt = 0;

        while (true)
        {
            attempt++;
            try
            {
                context.Tracer?.TraceInfo(context.OrchestrationId,GetType().Name, $"Attempt {attempt}");

                return await ExecuteCoreAsync(input, context);
            }
            catch (Exception ex) when (attempt <= RetryPolicy.MaxRetries)
            {
                context.Tracer?.TraceError(context.OrchestrationId, GetType().Name, ex);

                await Task.Delay(RetryPolicy.DelayBetweenRetries, context.CancellationToken);
            }
        }
    }

    /// <summary>
    /// ExecuteCoreAsync is an abstract method that must be implemented by derived classes to define the actual logic of the feature.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected abstract Task<FeatureResult<TResult>> ExecuteCoreAsync(TInput input, OrchestrationContext context);

}
