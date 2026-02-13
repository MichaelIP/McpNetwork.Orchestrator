namespace McpNetwork.Orchestrator.Interfaces
{
    public interface IAsyncResponseCompleter<TResponse>
        where TResponse : ICorrelatedResponse
    {
        void Complete(string correlationId, TResponse response);
    }
}
