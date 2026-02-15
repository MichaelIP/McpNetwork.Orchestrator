namespace McpNetwork.Orchestrator.Interfaces;

/// <summary>
/// Defines a contract for asynchronously awaiting a correlated response of type TResponse, identified by a correlation
/// ID.
/// </summary>
/// <remarks>Implementations of this interface are typically used in messaging or request/response scenarios where
/// responses are matched to requests using a correlation identifier. The interface allows consumers to asynchronously
/// wait for a specific response, with support for timeouts and cancellation.</remarks>
/// <typeparam name="TResponse">The type of response to await. Must implement ICorrelatedResponse.</typeparam>
public interface IAsyncResponseAwaiter<TResponse>
    where TResponse : ICorrelatedResponse
{
    /// <summary>
    /// Asynchronously waits for a response message that matches the specified correlation identifier, or until the
    /// timeout elapses or the operation is canceled.
    /// </summary>
    /// <remarks>If the response is not received within the specified timeout, the returned task is faulted
    /// with a TimeoutException. If the operation is canceled via the cancellation token, the task is
    /// canceled.</remarks>
    /// <param name="correlationId">The unique identifier used to correlate the response message. Cannot be null or empty.</param>
    /// <param name="timeout">The maximum duration to wait for the response before the operation times out.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the wait operation.</param>
    /// <returns>A task that represents the asynchronous wait operation. The task result contains the response message of type
    /// TResponse if received before the timeout or cancellation.</returns>
    Task<TResponse> WaitForResponseAsync(string correlationId,TimeSpan timeout,CancellationToken cancellationToken);
}
