using McpNetwork.Orchestrator.Interfaces;
using System.Collections.Concurrent;

namespace McpNetwork.Orchestrator.Orchestration;

/// <summary>
/// AsyncResponseAwaiter is a utility class that allows waiting for asynchronous responses based on correlation IDs. 
/// It maintains a concurrent dictionary of pending responses, where each correlation ID maps to a TaskCompletionSource 
/// that will be completed when the corresponding response is received. 
/// The WaitForResponseAsync method allows waiting for a response with a specified timeout and 
/// cancellation token, while the Complete method is used to complete the awaiting task when the 
/// response is received. 
/// This class is useful in scenarios where you need to correlate requests and responses in an 
/// asynchronous workflow, such as in an orchestration or messaging system.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public class AsyncResponseAwaiter<TResponse> : IAsyncResponseAwaiter<TResponse>, IAsyncResponseCompleter<TResponse>
    where TResponse : ICorrelatedResponse
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _pendingResponses = new();

    /// <summary>
    /// Waits for a response with the specified correlation ID, timeout, and cancellation token.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="timeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<TResponse> WaitForResponseAsync(string correlationId, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_pendingResponses.TryAdd(correlationId, tcs))
            throw new InvalidOperationException($"Duplicate correlationId {correlationId}");

        var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        timeoutCts.CancelAfter(timeout);

        timeoutCts.Token.Register(() =>
        {
            if (_pendingResponses.TryRemove(correlationId, out var pending))
            {
                pending.TrySetException(
                    new TimeoutException(
                        $"Timeout waiting for response {correlationId}"));
            }
        });

        return tcs.Task;
    }

    /// <summary>
    /// Completes the awaiting task for the specified correlation ID with the provided response.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="response"></param>
    public void Complete(string correlationId, TResponse response)
    {
        if (_pendingResponses.TryRemove(correlationId, out var tcs))
        {
            tcs.TrySetResult(response);
        }
    }
}
