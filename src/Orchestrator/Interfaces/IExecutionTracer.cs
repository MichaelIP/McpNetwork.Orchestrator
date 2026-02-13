namespace McpNetwork.Orchestrator.Interfaces
{

    public interface IExecutionTracer
    {
        IDisposable StartStep(string stepName, string correlationId);

        void RecordRetry(string stepName, int attempt, string reason);

        void RecordError(string stepName, string error);

        void TraceInfo(string correlationId, string stepName, string message);
        void TraceWarning(string correlationId, string stepName, string message);

        void TraceError(string correlationId, string stepName, Exception exception);

    }
}
