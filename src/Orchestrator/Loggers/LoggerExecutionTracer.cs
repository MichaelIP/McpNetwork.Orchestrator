using McpNetwork.Orchestrator.Helpers;
using McpNetwork.Orchestrator.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace McpNetwork.Orchestrator.Loggers;

public sealed class LoggerExecutionTracer : IExecutionTracer
{
    private readonly ILogger<LoggerExecutionTracer> _logger;

    public LoggerExecutionTracer(ILogger<LoggerExecutionTracer> logger)
    {
        _logger = logger;
    }

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

    public void RecordRetry(string stepName, int attempt,string reason)
    {
        _logger.LogWarning("STEP_RETRY {StepName} Attempt={Attempt} Reason={Reason}", stepName, attempt, reason);
    }

    public void RecordError(string stepName, string error)
    {
        _logger.LogError("STEP_ERROR {StepName} Error={Error}", stepName, error);
    } 

    public void TraceInfo(string correlationId, string stepName, string message)
    {
        _logger.LogInformation("TRACE_INFO {CorrelationId} {StepName} {Message}", correlationId, stepName, message);
    }

    public void TraceWarning(string correlationId, string stepName, string warning)
    {
        _logger.LogWarning("TRACE_WARNING {CorrelationId} {StepName} {Warning}", correlationId, stepName, warning);
    }

    public void TraceError(string correlationId, string stepName, Exception exception)
    {
        _logger.LogError(exception, "TRACE_ERROR {CorrelationId} {StepName} {ErrorMessage}", correlationId, stepName, exception.Message);
    }
}
