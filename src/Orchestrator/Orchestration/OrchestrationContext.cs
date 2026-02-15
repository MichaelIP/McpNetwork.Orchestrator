using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Orchestration;

public sealed class OrchestrationContext
{
    public CancellationToken CancellationToken { get; }

    public IExecutionTracer? Tracer { get; }
    public string OrchestrationId { get; }
    public DateTimeOffset StartedAt { get; }

    public OrchestrationContext(CancellationToken cancellationToken, IExecutionTracer? tracer, string? orchestrationId = null)
    {
        Tracer = tracer;
        StartedAt = DateTimeOffset.UtcNow;
        CancellationToken = cancellationToken;
        OrchestrationId = orchestrationId ?? Guid.NewGuid().ToString();
    }
}
