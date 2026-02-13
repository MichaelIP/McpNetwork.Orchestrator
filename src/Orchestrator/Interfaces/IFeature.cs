using McpNetwork.Orchestrator.Features;
using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces
{
    public interface IFeature<TInput, TResult>
    {
        Task<FeatureResult<TResult>> ExecuteAsync(TInput input, OrchestrationContext context);
    }

    public interface IRetriableFeature<TInput, TResult> : IFeature<TInput, TResult>
    {
        RetryPolicy RetryPolicy { get; }
    }

}
