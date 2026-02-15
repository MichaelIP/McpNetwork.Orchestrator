namespace McpNetwork.Orchestrator.Interfaces;


/// <summary>
/// Defines methods for tracing the execution of operations, including steps, retries, informational messages, warnings,
/// and errors.
/// </summary>
/// <remarks>Implementations of this interface can be used to monitor and record the progress and issues
/// encountered during the execution of complex workflows or operations. This is useful for diagnostics, auditing, and
/// performance analysis. The interface is designed to be used in scenarios where tracking the flow and outcome of
/// execution steps is important, such as distributed systems or long-running processes.</remarks>
public interface IExecutionTracer
{
    /// <summary>
    /// Indicates the start of a new execution step with the given name and correlation ID. The returned IDisposable should be
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    IDisposable StartStep(string stepName, string correlationId);

    /// <summary>
    /// Indicates that a retry attempt is being made for a step with the given name, attempt number, and reason for the retry. This can be used to log retry attempts and their causes.
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="attempt"></param>
    /// <param name="reason"></param>
    void RecordRetry(string stepName, int attempt, string reason);

    /// <summary>
    /// Indicates that an error has occurred in a step with the given name and error message. This can be used to log errors that occur during execution steps for later analysis and troubleshooting.
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="error"></param>
    void RecordError(string stepName, string error);

    /// <summary>
    /// Indicates that an informational message should be recorded for a step with the given name and correlation ID. This can be used to log important information about the execution of steps, such as state changes, decisions made, or other relevant details that may be useful for understanding the flow of execution.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="message"></param>
    void TraceInfo(string correlationId, string stepName, string message);

    /// <summary>
    /// Indicates that a warning message should be recorded for a step with the given name and correlation ID. This can be used to log potential issues or important notices that may not be errors but are still relevant to the execution of steps, such as deprecated features, performance concerns, or other conditions that warrant attention.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="message"></param>
    void TraceWarning(string correlationId, string stepName, string message);

    /// <summary>
    /// Indicates that an error message should be recorded for a step with the given name and correlation ID, along with the associated exception. This can be used to log detailed information about errors that occur during execution steps, including stack traces and exception details, which can be invaluable for diagnosing and resolving issues in complex workflows.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="exception"></param>
    void TraceError(string correlationId, string stepName, Exception exception);

}
