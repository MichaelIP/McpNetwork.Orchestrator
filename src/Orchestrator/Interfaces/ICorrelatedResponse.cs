namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Represents a response that is associated with a correlation identifier for tracking or matching related requests and
/// responses.
/// </summary>
/// <remarks>Implementations of this interface enable correlation of responses with their originating requests,
/// which is useful in distributed systems, messaging scenarios, or asynchronous operations where tracking the flow of a
/// request is required.</remarks>
public interface ICorrelatedResponse
{
    /// <summary>
    /// Gets the unique identifier used to correlate related operations or messages.
    /// </summary>
    string CorrelationId { get; }
}
