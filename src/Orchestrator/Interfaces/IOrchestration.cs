using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces
{
    public interface IOrchestration<TResult>
    {
        Task<TResult> ExecuteAsync(OrchestrationContext context);
    }
}
