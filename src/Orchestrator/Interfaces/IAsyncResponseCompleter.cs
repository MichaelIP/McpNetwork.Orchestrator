namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Defines a contract for asynchronously completing a response associated with a specific correlation identifier.
/// </summary>
/// <typeparam name="TResponse">The type of response to complete. Must implement <see cref="ICorrelatedResponse"/>.</typeparam>
public interface IAsyncResponseCompleter<TResponse>
    where TResponse : ICorrelatedResponse
{
    /// <summary>
    /// Marks the operation identified by the specified correlation ID as complete and provides the associated response.
    /// </summary>
    /// <param name="correlationId">The unique identifier that correlates the operation to be completed. Cannot be null or empty.</param>
    /// <param name="response">The response object to associate with the completed operation.</param>
    void Complete(string correlationId, TResponse response);
}
