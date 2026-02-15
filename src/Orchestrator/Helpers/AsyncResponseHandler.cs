using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Helpers;

/// <summary>
/// Handles asynchronous responses by mapping raw messages to strongly typed responses and completing the associated
/// response tasks.
/// </summary>
/// <remarks>This class is typically used in messaging or RPC scenarios where responses must be correlated with
/// requests. It relies on a user-provided mapping function to convert raw messages to response objects and an
/// IAsyncResponseCompleter to signal completion. Thread safety depends on the implementations of the provided completer
/// and mapper.</remarks>
/// <typeparam name="TResponse">The type of response that implements the ICorrelatedResponse interface and contains correlation information.</typeparam>
public class AsyncResponseHandler<TResponse>
    where TResponse : ICorrelatedResponse
{
    private readonly IAsyncResponseCompleter<TResponse> _completer;
    private readonly Func<object, TResponse> _mapper;

    /// <summary>
    /// Initializes a new instance of the AsyncResponseHandler class with the specified response completer and response
    /// mapping function.
    /// </summary>
    /// <param name="completer">An object that handles the completion of asynchronous responses.</param>
    /// <param name="mapper">A function that maps a raw response object to a strongly typed response of type TResponse. Cannot be null.</param>
    public AsyncResponseHandler(IAsyncResponseCompleter<TResponse> completer, Func<object, TResponse> mapper)
    {
        _completer = completer;
        _mapper = mapper;
    }

    /// <summary>
    /// Processes an incoming raw message and completes the associated operation with the mapped response.
    /// </summary>
    /// <remarks>This method is typically called by the message transport layer when a new message is
    /// received. The message is mapped to a response and used to complete any pending operation that matches the
    /// response's correlation identifier.</remarks>
    /// <param name="rawMessage">The raw message object to process. Must not be null. The format and type are expected to be compatible with the
    /// configured message mapper.</param>
    public void OnMessageReceived(object rawMessage)
    {
        var response = _mapper(rawMessage);

        _completer.Complete(
            response.CorrelationId,
            response);
    }
}
