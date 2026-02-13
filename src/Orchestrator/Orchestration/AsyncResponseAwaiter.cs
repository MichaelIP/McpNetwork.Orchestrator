using McpNetwork.Orchestrator.Interfaces;
using System.Collections.Concurrent;

namespace McpNetwork.Orchestrator.Orchestration
{
    public class AsyncResponseAwaiter<TResponse> : IAsyncResponseAwaiter<TResponse>, IAsyncResponseCompleter<TResponse>
        where TResponse : ICorrelatedResponse
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _pendingResponses = new();

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

        public void Complete(string correlationId, TResponse response)
        {
            if (_pendingResponses.TryRemove(correlationId, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }
    }

}
