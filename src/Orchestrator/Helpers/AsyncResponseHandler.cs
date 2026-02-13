using McpNetwork.Orchestrator.Interfaces;

namespace McpNetwork.Orchestrator.Helpers
{
    public class AsyncResponseHandler<TResponse>
        where TResponse : ICorrelatedResponse
    {
        private readonly IAsyncResponseCompleter<TResponse> _completer;
        private readonly Func<object, TResponse> _mapper;

        public AsyncResponseHandler(
            IAsyncResponseCompleter<TResponse> completer,
            Func<object, TResponse> mapper)
        {
            _completer = completer;
            _mapper = mapper;
        }

        public void OnMessageReceived(object rawMessage)
        {
            var response = _mapper(rawMessage);

            _completer.Complete(
                response.CorrelationId,
                response);
        }
    }


}
