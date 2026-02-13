using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces;

public interface IOrchestrationStep
{
    Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state);
}
