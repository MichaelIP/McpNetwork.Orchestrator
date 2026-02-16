using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// OrchestrationContext provides contextual information and utilities for executing orchestration steps. 
/// This context is passed to each orchestration step, allowing them to access necessary information and 
/// utilities for their execution.
/// </summary>
public sealed class OrchestrationContext
{
    /// <summary>
    /// CancellationToken that can be used by orchestration steps to observe cancellation requests.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    /// Execution tracer that can be used by orchestration steps to trace their execution, record retries, and log errors.
    /// </summary>
    public IExecutionTracer? Tracer { get; }
    /// <summary>
    /// OrchestrationId is a unique identifier for the orchestration instance.
    /// </summary>
    public string OrchestrationId { get; }
    /// <summary>
    /// StartedAt is a timestamp indicating when the orchestration was initiated. 
    /// </summary>
    public DateTimeOffset StartedAt { get; }

    /// <summary>
    /// Initializes a new instance of the OrchestrationContext class
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="tracer"></param>
    /// <param name="orchestrationId"></param>
    public OrchestrationContext(CancellationToken cancellationToken, IExecutionTracer? tracer, string? orchestrationId = null)
    {
        Tracer = tracer;
        StartedAt = DateTimeOffset.UtcNow;
        CancellationToken = cancellationToken;
        OrchestrationId = orchestrationId ?? Guid.NewGuid().ToString();
    }
}
