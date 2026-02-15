using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Defines a contract for a feature that can be executed within an orchestration context, taking an input of type TInput
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IFeature<TInput, TResult>
{
    /// <summary>
    /// Executes the feature asynchronously, taking an input of type TInput and an orchestration context, and returns a FeatureResult containing the result of type TResult
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<FeatureResult<TResult>> ExecuteAsync(TInput input, OrchestrationContext context);
}

/// <summary>
/// Defines a contract for a feature that supports retry logic, allowing it to be executed within an orchestration context with
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IRetriableFeature<TInput, TResult> : IFeature<TInput, TResult>
{
    /// <summary>
    /// Exposes the retry policy associated with this feature, which defines the conditions and parameters for retrying the feature execution in case of failure. This allows the orchestration engine to automatically handle retries based on the specified policy when executing this feature.
    /// </summary>
    RetryPolicy RetryPolicy { get; }
}
