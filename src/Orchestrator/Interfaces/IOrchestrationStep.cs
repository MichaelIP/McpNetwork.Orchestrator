using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Defines a contract for an orchestration step, which represents a single unit of work or action that can be executed as part of an orchestration workflow. The ExecuteAsync method encapsulates the logic for performing the step's operation, taking into account the current orchestration context and state, and returning a StepResult that indicates the outcome of the execution and any necessary information for proceeding to the next step in the workflow.
/// </summary>
public interface IOrchestrationStep
{
    /// <summary>
    /// Executes the orchestration step asynchronously, using the provided orchestration context and state. The method should contain the logic for performing the specific action associated with this step, and it should return a StepResult that indicates whether the step was successful and whether the orchestration should continue to the next step or if it should fail with a specific reason. The implementation of this method will depend on the specific requirements of the step and the overall orchestration workflow.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    Task<StepResult> ExecuteAsync(OrchestrationContext context, OrchestrationState state);
}
