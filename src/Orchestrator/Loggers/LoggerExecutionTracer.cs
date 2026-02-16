using McpNetwork.Orchestrator.Helpers;
using McpNetwork.Orchestrator.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace McpNetwork.Orchestrator.Loggers;

/// <summary>
/// Generic execution tracer that logs step execution, retries, errors, and custom trace messages 
/// </summary>
public sealed class LoggerExecutionTracer : IExecutionTracer
{
    private readonly ILogger<LoggerExecutionTracer> _logger;

    /// <summary>
    /// Initializes a new instance of the LoggerExecutionTracer class with the provided logger. 
    /// </summary>
    /// <param name="logger"></param>
    public LoggerExecutionTracer(ILogger<LoggerExecutionTracer> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Starts a step with the given name and correlation ID, logging the start of the step
    /// This allows for easy tracking of step execution times and correlation across different steps using the correlation ID.
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    public IDisposable StartStep(string stepName, string correlationId)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("STEP_START {StepName} {CorrelationId}", stepName, correlationId);

        return new DisposableAction(() =>
        {
            stopwatch.Stop();
            _logger.LogInformation("STEP_END {StepName} {CorrelationId} {DurationMs}", stepName, correlationId, stopwatch.ElapsedMilliseconds);
        });
    }

    /// <summary>
    /// Records a retry attempt for a step with the given name, attempt number, and reason. 
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="attempt"></param>
    /// <param name="reason"></param>
    public void RecordRetry(string stepName, int attempt,string reason)
    {
        _logger.LogWarning("STEP_RETRY {StepName} Attempt={Attempt} Reason={Reason}", stepName, attempt, reason);
    }

    /// <summary>
    /// Trace an error for a step with the given name and error details. 
    /// </summary>
    /// <param name="stepName"></param>
    /// <param name="error"></param>
    public void RecordError(string stepName, string error)
    {
        _logger.LogError("STEP_ERROR {StepName} Error={Error}", stepName, error);
    }

    /// <summary>
    /// Trace an informational message with the given correlation ID, step name, and information details.
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="message"></param>
    public void TraceInfo(string correlationId, string stepName, string message)
    {
        _logger.LogInformation("TRACE_INFO {CorrelationId} {StepName} {Message}", correlationId, stepName, message);
    }

    /// <summary>
    /// Trace a warning message with the given correlation ID, step name, and warning details. 
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="warning"></param>
    public void TraceWarning(string correlationId, string stepName, string warning)
    {
        _logger.LogWarning("TRACE_WARNING {CorrelationId} {StepName} {Warning}", correlationId, stepName, warning);
    }

    /// <summary>
    /// Traces an error with the given correlation ID, step name, and exception. 
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="stepName"></param>
    /// <param name="exception"></param>
    public void TraceError(string correlationId, string stepName, Exception exception)
    {
        _logger.LogError(exception, "TRACE_ERROR {CorrelationId} {StepName} {ErrorMessage}", correlationId, stepName, exception.Message);
    }
}
