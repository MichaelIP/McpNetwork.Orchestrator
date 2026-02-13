namespace McpNetwork.Orchestrator.Interfaces
{
    public interface IAsyncResponseAwaiter<TResponse>
        where TResponse : ICorrelatedResponse
    {
        Task<TResponse> WaitForResponseAsync(string correlationId,TimeSpan timeout,CancellationToken cancellationToken);
    }

}
