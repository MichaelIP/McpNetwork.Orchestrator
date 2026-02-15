using McpNetwork.Orchestrator.Orchestration;

namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Declares a contract for an orchestration, which represents a workflow or process that can be executed within an orchestration context to produce a result of type TResult. An orchestration typically consists of multiple steps or features that are executed in a defined sequence, and the ExecuteAsync method encapsulates the logic for running the entire orchestration and returning the final result.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IOrchestration<TResult>
{
    /// <summary>
    /// Executes the orchestration asynchronously within the provided orchestration context and returns a result of type TResult. The orchestration context may contain information such as cancellation tokens, tracing capabilities, and other relevant data needed for the execution of the orchestration. The method is designed to be asynchronous to allow for non-blocking execution of potentially long-running workflows or processes.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<TResult> ExecuteAsync(OrchestrationContext context);
}
